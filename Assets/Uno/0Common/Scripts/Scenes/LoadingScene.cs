using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : BaseScene
{
    public static string nextScene;
    Image progressBarImage;

    protected override void init()
    {
        base.init();

        Managers.Sound.Play("ButtonClick", Define.Sound.Effect);

        ScenType = Define.Scene.Loading; // here is Loadging Scene

        Managers.UI.ShowScene<UI_LoadingVideo>();
        progressBarImage = UI_LoadingVideo.FindFirstObjectByType<Image>();

        nextScene = Managers.Scene.nextSceneName;
        StartCoroutine(CoMoveToNextScene());
        
    }

    IEnumerator CoMoveToNextScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        progressBarImage.fillAmount = 0;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBarImage.fillAmount = Mathf.Lerp(progressBarImage.fillAmount, op.progress, timer);
                if (progressBarImage.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBarImage.fillAmount = Mathf.Lerp(progressBarImage.fillAmount, 1f, timer);
                if (progressBarImage.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
        Managers.Scene.LoadScene(nextScene);
    }


    public override void Clear()
    {
        Debug.Log("Loading Scene Clear!!!");
    }
}
