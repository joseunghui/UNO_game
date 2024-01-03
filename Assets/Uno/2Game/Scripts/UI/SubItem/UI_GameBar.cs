using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameBar : UI_SubItem
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Define.Texts));
        Bind<Button>(typeof(Define.Buttons));

        GetButton((int)Define.Buttons.OptionBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.Sound.Play("ButtonClick", Define.Sound.Effect);
            Managers.UI.ShowPopup<UI_Option>();
        });
    }

}
