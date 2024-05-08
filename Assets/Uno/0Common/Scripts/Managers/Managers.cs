using BackEnd;
using Battlehub.Dispatcher;
using System;
using System.Collections;
using UnityEngine;

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


    private void Awake()
    {
        var bro = Backend.Initialize(true);

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro);
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro);
        }

        Init();
    }
    void Start()
    {
        
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
                go.AddComponent<SendQueueMgr>();
                go.AddComponent<BackEndMatchManager>();
                go.AddComponent<DataParser>();
                go.AddComponent<Dispatcher>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            // sound
            s_instance._sound.Init();
        }
    }



    private void Update()
    {
        //비동기함수 풀링
        Backend.AsyncPoll();
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