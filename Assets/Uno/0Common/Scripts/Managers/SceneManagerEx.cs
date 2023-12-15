using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : MonoBehaviour
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public string nextSceneName;

    // Scene Change Method
    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();

        nextSceneName = GetSceneName(type);

        // �ϴ� �ε� ������ �̵� 
        SceneManager.LoadScene(GetSceneName(Define.Scene.Loading));
    }

    public void LoadScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
    }

    // Enum�� �� Ÿ���� string���� ��������
    public string GetSceneName(Define.Scene type)
    {
        return System.Enum.GetName(typeof(Define.Scene), type);
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
