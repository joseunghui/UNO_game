using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    UserInfoData data;
    // 게임 모드 Base에서 선언
    public GameMode.PVCMode gameMode;

    protected override void init()
    {
        base.init();

        // BGM
        Managers.Sound.Play("GameBGM", Define.Sound.BGM);

        Managers.UI.ShowPopup<UI_LevelSelect>();
        UI_GameBar gameBar = Managers.UI.MakeSubItemInOldCanvas<UI_GameBar>();
        gameBar.transform.localScale = Vector3.one;
        gameBar.transform.localPosition = new Vector3(0, 0, 0);
        
    }

    public override void Clear()
    {
        Debug.Log("Game Scene Clear!!!");
    }


}
