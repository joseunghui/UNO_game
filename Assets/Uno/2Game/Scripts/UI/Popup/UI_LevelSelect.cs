using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LevelSelect : UI_Popup
{
    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();

        // game BGM Stop
        Managers.Sound.Clear();

        Bind<Button>(typeof(Define.Buttons));

        GetButton((int)Define.Buttons.EasyBtn).gameObject.BindEvent((PointerEventData) =>
        {
            StartCoroutine(CoButtonClickSoundPlay());
        });

        GetButton((int)Define.Buttons.NormalBtn).gameObject.BindEvent((PointerEventData) =>
        {
            StartCoroutine(CoButtonClickSoundPlay());
        });

        GetButton((int)Define.Buttons.HardBtn).gameObject.BindEvent((PointerEventData) =>
        {
            StartCoroutine(CoButtonClickSoundPlay());
        });
    }

    IEnumerator CoButtonClickSoundPlay()
    {
        yield return null;
        Managers.Sound.Play("ButtonClick", Define.Sound.Effect);
        Managers.UI.ClosePopup(this);
    }

}
