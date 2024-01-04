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

        Bind<Button>(typeof(Define.Buttons));

        GetButton((int)Define.Buttons.CloseBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.UI.ClosePopup();
        });
    }
}
