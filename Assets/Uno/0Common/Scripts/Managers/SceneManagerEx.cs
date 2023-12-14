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

        // 일단 로딩 씬으로 이동 
        SceneManager.LoadScene(GetSceneName(Define.Scene.Loading));


        SceneManager.LoadScene(GetSceneName(type));

    }

    

    // Enum속 씬 타입을 string으로 가져오기
    public string GetSceneName(Define.Scene type)
    {
        return System.Enum.GetName(typeof(Define.Scene), type);
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
