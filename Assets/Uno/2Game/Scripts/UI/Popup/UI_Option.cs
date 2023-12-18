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

        GetButton((int)Define.Buttons.RestartGameBtn).gameObject.BindEvent((PointerEventData) =>
        {
            PVCGameController controller = Utill.GetOrAddComponent<PVCGameController>(gameObject);

            Debug.Log("Restart Button Click");
        });

        GetButton((int)Define.Buttons.StopGameBtn).gameObject.gameObject.BindEvent((PointerEventData) =>
        {
            Debug.Log("Stop Button Click");
        });
    }
}
