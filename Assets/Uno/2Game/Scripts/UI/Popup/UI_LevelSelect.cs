using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LevelSelect : UI_Popup
{
    PVCGameController controller;

    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();

        controller = Utill.GetOrAddComponent<PVCGameController>(gameObject);
        // game BGM Stop
        Managers.Sound.Clear();

        Bind<Button>(typeof(Define.Buttons));

        GetButton((int)Define.Buttons.EasyBtn).gameObject.BindEvent((PointerEventData) =>
        {
            StartCoroutine("CoButtonClickSoundPlay", GameMode.PVCMode.EASY);
        });

        GetButton((int)Define.Buttons.NormalBtn).gameObject.BindEvent((PointerEventData) =>
        {
            StartCoroutine("CoButtonClickSoundPlay", GameMode.PVCMode.NORMAL);
        });

        GetButton((int)Define.Buttons.HardBtn).gameObject.BindEvent((PointerEventData) =>
        {
            StartCoroutine("CoButtonClickSoundPlay", GameMode.PVCMode.HARD);
        });
    }

    IEnumerator CoButtonClickSoundPlay(GameMode.PVCMode mode)
    {
        yield return controller.GameLevel = mode;

        Managers.Sound.Play("ButtonClick", Define.Sound.Effect);
        yield return null;
        Managers.Sound.Play("GameBGM", Define.Sound.BGM);

        Managers.UI.ClosePopup();
    }

}
