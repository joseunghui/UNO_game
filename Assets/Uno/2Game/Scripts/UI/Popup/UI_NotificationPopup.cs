using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NotificationPopup : UI_Popup
{
    Image image;

    float time = 0f;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Define.Images));
        image = GetImage((int)Define.Images.NotiImage);

        StartCoroutine(CoDisappearPopup());
    }

    IEnumerator CoDisappearPopup()
    {
        if (image == null)
            yield break;

        while (time <= 1.0f) 
        {
            yield return null;
            image.transform.localScale = Vector3.one * (1 - time);
            time += Time.deltaTime;
        }

        Destroy(gameObject);
    }

}