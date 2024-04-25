using BackEnd;
using Battlehub.Dispatcher;
using System;
using System.Collections;
using UnityEngine;
using static BackEnd.SendQueue;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성이 보장된다
    static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

    DataManager _data = new DataManager();
    ResourceManager _resource = new ResourceManager();
    SoundManager _sound = new SoundManager();
    SceneManagerEx _scene = new SceneManagerEx();
    UIManager _ui = new UIManager();
    InputManager _input = new InputManager();
    BackEndMatchManager _match = new BackEndMatchManager();

    public static DataManager Data { get { return Instance._data; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static InputManager Input { get {  return Instance._input; } }
    public static BackEndMatchManager Match { get {  return Instance._match; } }

    void ExceptionHandler(Exception e)
    {
        // 예외 처리
    }

    void Start()
    {
        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            s_instance._match.Init();

            Debug.Log("초기화 성공 : " + bro);

            // SendQueue 초기화
            StartSendQueue(true, ExceptionHandler);
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro);
        }

        Init();
    }


    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
                go.AddComponent<Dispatcher>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            // data
            // s_instance._data.Init();

            // sound
            s_instance._sound.Init();
        }
    }


    private void Update()
    {
        Backend.AsyncPoll();
    }

    void OnApplicationQuit()
    {
        // 어플리케이션이 종료되었을 때 SendQueue를 정지 시킴
        BackEnd.SendQueue.StopSendQueue();

        if (s_instance._match.isConnectMatchServer)
        {
            s_instance._match.AccessMatchServer();
            Debug.Log("ApplicationQuit - LeaveMatchServer");
        }
    }

    void OnApplicationPause(bool isPause)
    {
        if (isPause == false)
        {
            // 어플리케이션이 재실행 되었을 때 SendQueue를 재실행 시킴
            SendQueue.ResumeSendQueue();
        }
        else
        {
            // 어플리케이션이 정지되었을 때 SendQueue를 일시 정지 시킴 
            SendQueue.PauseSendQueue();
        }
    }


    // 씬 이동 시 없애줘야 하는 것들을 한방에 없애기 
    // 호출은 SceneManagerEx.cs 에서
    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
    }
}