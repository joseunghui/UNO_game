using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_NotificationPopup : UI_Popup
{
    Image image;

    float time = 0f;

    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();

        Bind<GameObject>(typeof(Define.NotiPopup));

        GetGameObject((int)Define.NotiPopup.UI_NotificationPopup);
        image = GetImage((int)Define.NotiPopup.NotiImage);
        GetText((int)Define.NotiPopup.NotiText);

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
