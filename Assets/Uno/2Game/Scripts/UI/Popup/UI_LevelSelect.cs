using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LevelSelect : UI_Popup
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        // game BGM Stop
        Managers.Sound.Clear();

        Bind<TextMeshProUGUI>(typeof(Define.Texts));
        Bind<Button>(typeof(Define.Buttons));

        GetButton((int)Define.Buttons.EasyBtn).gameObject.BindEvent((PointerEventData) =>
        {
            StartCoroutine(CoButtonClickSoundPlay());
            SetPVCGameMode(GameMode.PVCMode.EASY);
        });

        GetButton((int)Define.Buttons.NormalBtn).gameObject.BindEvent((PointerEventData) =>
        {
            StartCoroutine(CoButtonClickSoundPlay());
            SetPVCGameMode(GameMode.PVCMode.NORMAL);
        });

        GetButton((int)Define.Buttons.HardBtn).gameObject.BindEvent((PointerEventData) =>
        {
            StartCoroutine(CoButtonClickSoundPlay());
            SetPVCGameMode(GameMode.PVCMode.HARD);
        });

    }

    IEnumerator CoButtonClickSoundPlay()
    {
        Managers.Sound.Play("ButtonClick", Define.Sound.Effect);
        yield return null;
        Managers.Sound.Play("GameBGM", Define.Sound.BGM);
        yield return null;
    }

    void SetPVCGameMode(GameMode.PVCMode mode)
    {
        GameScene gameScene = GameObject.FindWithTag("Scene").GetComponent<GameScene>();
        CardController cc = gameScene.GetComponent<CardController>();

        switch (mode)
        {
            case GameMode.PVCMode.EASY:
                gameScene.gameMode = GameMode.PVCMode.EASY;
                break;
            case GameMode.PVCMode.NORMAL:
                gameScene.gameMode= GameMode.PVCMode.NORMAL;
                break;
            case GameMode.PVCMode.HARD:
                gameScene.gameMode = GameMode.PVCMode.HARD;
                break;
        }

        UI_GameMode gameMode = Managers.UI.MakeSubItemInTop<UI_GameMode>();
        gameMode.transform.localScale = Vector3.one;
        gameMode.transform.localPosition = new Vector3(-550f, -90f, 0);
        gameMode.SetGameModeSetting(mode);

        UI_GameBar_Timer gameBarTimer = Managers.UI.MakeSubItemInTop<UI_GameBar_Timer>();
        gameBarTimer.transform.localScale = Vector3.one;
        gameBarTimer.transform.localPosition = new Vector3(-200f, -90f, 0);
        gameBarTimer.SetTimer("10");

        // StartCardCount 설정
        cc.SetStartCardCountbyGameMode(null);

        Managers.UI.ClosePopup();
    }

}
