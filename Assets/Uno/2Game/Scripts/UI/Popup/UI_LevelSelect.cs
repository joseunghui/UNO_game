using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LevelSelect : UI_Popup
{
    PVCGameController controller;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        // controller = Utill.GetOrAddComponent<PVCGameController>(gameObject);

        //Bind<UI_GameMode>(typeof(UI_GameMode));
        //UI_GameMode _gameMode = Get<UI_GameMode>((int)Define.UI_SubItems.UI_GameMode);
        //Debug.Log($"UI_GameMode >> {_gameMode.name}"); //TODO 

        // game BGM Stop
        Managers.Sound.Clear();

        Bind<TextMeshProUGUI>(typeof(Define.Texts));
        Bind<Button>(typeof(Define.Buttons));

        GetButton((int)Define.Buttons.EasyBtn).gameObject.BindEvent((PointerEventData) =>
        {
            UI_GameMode gameMode = Managers.UI.MakeSubItem<UI_GameMode>();
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
        Managers.Sound.Play("ButtonClick", Define.Sound.Effect);
        yield return null;
        Managers.Sound.Play("GameBGM", Define.Sound.BGM);
        yield return null;

        // controller.SetPVCGameLevel(mode);

        UI_Card cardObject = Managers.UI.ShowSceneInOldCanvas<UI_Card>(IsContent: true);
        //UI_Card cardObject = Managers.UI.MakeSubItemInContent<UI_Card>();
        cardObject.gameObject.transform.localScale = Vector3.one;

        UI_Turn turnButtons = Managers.UI.MakeSubItemInContent<UI_Turn>();
        turnButtons.transform.localScale = Vector3.one;

        Managers.UI.ClosePopup();
    }

}
