using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : MonoBehaviour
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }


    // Scene Change Method
    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();

        Debug.Log($"type : {type}");

        // �ϴ� �ε� ������ �̵� 
        SceneManager.LoadScene(GetSceneName(Define.Scene.Loading));


        SceneManager.LoadScene(GetSceneName(type));

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
