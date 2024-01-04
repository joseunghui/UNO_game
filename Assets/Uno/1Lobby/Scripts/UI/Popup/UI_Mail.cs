using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Mail : UI_Popup
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(Define.Groups));
        Bind<Button>(typeof(Define.Buttons));

        SetMailData();

        GetButton((int)Define.Buttons.CloseBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.UI.ClosePopup();
        });

        GetButton((int)Define.Buttons.ReceiveAllBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Debug.Log("Receive all mail Button Click!");
        });
    }

    void SetMailData()
    {
        // mail data 
        GameObject mailList = Get<GameObject>((int)Define.Groups.MailList);

        // 혹시 모르니 기존 데이터 날리기
        foreach (Transform child in mailList.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < 10; i++)
        {
            GameObject Mail_EA = Managers.UI.MakeSubItem<UI_Mail_EA>(parent: mailList.transform).gameObject;

            Mail_EA.transform.SetParent(mailList.transform);
            Mail_EA.transform.localScale = Vector3.one;

            UI_Mail_EA mailItem = Mail_EA.GetOrAddComponent<UI_Mail_EA>();
            mailItem.SetMailEAData((i%2 == 0 ? Define.Goods.Dia : Define.Goods.Heart), 10, "샘플입니다아아아아아아아아앙 >> 메일 설명");
        }
    }
}
