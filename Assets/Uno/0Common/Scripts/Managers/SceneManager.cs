using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(Define.Scene type)
    {
        // �ٸ� �� �ε� �� ���ʿ��� �޸� �ѹ濡 �����ֱ�
        Managers.Clear();

        SceneManager.LoadScene(GetSceneName(type));
    }

    // Enum�� �� Ÿ���� string���� ��������
    string GetSceneName(Define.Scene type)
    {
        return System.Enum.GetName(typeof(Define.Scene), type);
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

}