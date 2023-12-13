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

        StartCoroutine("LoadScene", GetSceneName(type));
    }

    IEnumerator LoadScene(string nextSceneName = null)
    {
        if (nextSceneName == null)
            yield break;

        SceneManager.LoadScene(nextSceneName);

        UI_ProgressBar progressBar = null;
        progressBar.Progress(SceneManager.LoadSceneAsync(nextSceneName));
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
