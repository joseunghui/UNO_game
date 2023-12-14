using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ProgressBar : UI_Popup
{
    Image progress;

    private void Start()
    {
        init();
    }

    public override void init()
    {
        Bind<GameObject>(typeof(Define.Progress));
        Bind<Image>(typeof(Define.Progress));

        progress = GetImage((int)Define.Progress.ProgressBar).GetComponent<Image>();

        // progress bar connect
        // StartCoroutine(Progress());
    }

    public IEnumerator Progress(AsyncOperation op = null)
    {
        float timer = 0.0f;

        while (!op.isDone)
        {
            timer += Time.deltaTime;

            if (op.progress < 0.9f)
            {
                progress.fillAmount = Mathf.Lerp(progress.fillAmount, op.progress, timer);
                if (progress.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progress.fillAmount = Mathf.Lerp(progress.fillAmount, 1f, timer);
                if (progress.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
