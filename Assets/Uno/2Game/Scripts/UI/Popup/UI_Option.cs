using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Option : UI_Popup
{
    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();

        Bind<Button>(typeof(Define.Buttons));

        GetButton((int)Define.Buttons.CloseBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.UI.ClosePopup(this);
        });
    }
}
