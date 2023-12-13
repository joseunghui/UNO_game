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
        CurrentScene.Clear();

        Debug.Log($"type : {type}");
        

        StartCoroutine("BeforeLoadScene", GetSceneName(type));

    }

    IEnumerator BeforeLoadScene(string nextSceneName)
    {
        Debug.Log($"nextSceneName : {nextSceneName}");

        if (nextSceneName == null)
            yield break;

        SceneManager.LoadScene(nextSceneName);

        UI_ProgressBar progressBar = null;
        progressBar.Progress(SceneManager.LoadSceneAsync(nextSceneName));
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
