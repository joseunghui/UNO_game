using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UI_NotificationPopup : UI_Popup
{
    float time = 1.0f;



    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();


        Bind<GameObject>(typeof(Define.NotiPopup));

        GetGameObject((int)Define.NotiPopup.UI_NotificationPopup);
        GetText((int)Define.NotiPopup.NotiText);

        StartCoroutine(DisappearPopup());
    }

    private IEnumerator DisappearPopup()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);

        if (transform == null)
            yield break;


        yield return new WaitForSeconds(1.2f);
    }
}
