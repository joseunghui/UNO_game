using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // ���ϼ��� ����ȴ�
    static Managers Instance { get { Init(); return s_instance; } } // ������ �Ŵ����� ����´�

    ResourceManager _resource = new ResourceManager();
    SoundManager _sound = new SoundManager();
    SceneManagerEx _scene = new SceneManagerEx();
    UIManager _ui = new UIManager();

    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static UIManager UI { get { return Instance._ui; } }

    // Start is called before the first frame update
    void Start()
    {
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
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            // sound
            s_instance._sound.init();

        }
    }

    // �� �̵� �� ������� �ϴ� �͵��� �ѹ濡 ���ֱ� 
    // ȣ���� SceneManagerEx.cs ����
    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
    }
}