using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(Define.Scene type)
    {
        // 다른 씬 로드 시 불필요한 메모리 한방에 날려주기
        Managers.Clear();

        SceneManager.LoadScene(GetSceneName(type));
    }

    // Enum속 씬 타입을 string으로 가져오기
    string GetSceneName(Define.Scene type)
    {
        return System.Enum.GetName(typeof(Define.Scene), type);
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

}