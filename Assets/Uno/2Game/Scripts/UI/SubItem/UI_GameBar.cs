using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameBar : UI_SubItem
{
    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();

        Bind<Button>(typeof(Define.Buttons));

        GetButton((int)Define.Buttons.OptionBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.Sound.Play("ButtonClick", Define.Sound.Effect);
            Managers.UI.ShowPopup<UI_Option>();
        });

        
    }
}
