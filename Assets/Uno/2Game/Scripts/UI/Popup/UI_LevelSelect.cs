using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LevelSelect : UI_Popup
{
    public int gameLevel;

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
            gameLevel = (int)GameMode.PVCMode.EASY;
            StartCoroutine(CoButtonClickSoundPlay());
        });

        GetButton((int)Define.Buttons.NormalBtn).gameObject.BindEvent((PointerEventData) =>
        {
            gameLevel = (int)GameMode.PVCMode.NORMAL;
            StartCoroutine(CoButtonClickSoundPlay());
        });

        GetButton((int)Define.Buttons.HardBtn).gameObject.BindEvent((PointerEventData) =>
        {
            gameLevel = (int)GameMode.PVCMode.HARD;
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
