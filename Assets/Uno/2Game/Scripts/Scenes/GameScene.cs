using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        ScenType = Define.Scene.Game; // here is Game Scene

        Managers.UI.ShowPopup<UI_LevelSelect>();

        UI_GameBar gameBar = Managers.UI.MakeSubItemInTop<UI_GameBar>();
        gameBar.transform.localScale = Vector3.one;
        gameBar.transform.localPosition = Vector3.zero;

        // UI_GameBar_Timer gameBarTimer = Managers.UI.MakeSubItem<UI_GameBar_Timer>();
        UI_GameBar_Timer gameBarTimer = Managers.UI.MakeSubItemInTop<UI_GameBar_Timer>();
        gameBarTimer.transform.localScale = Vector3.one;
        gameBarTimer.transform.localPosition = Vector3.zero;

        // BGM
        Managers.Sound.Play("GameBGM", Define.Sound.BGM);
    }

    public override void Clear()
    {
        Debug.Log("Game Scene Clear!!!");
    }


}
