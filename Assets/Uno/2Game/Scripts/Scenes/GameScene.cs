using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{

    protected override void init()
    {
        base.init();

        ScenType = Define.Scene.Game; // here is Game Scene

        Managers.UI.ShowPopup<UI_LevelSelect>();

        UI_GameBar gameBar = Managers.UI.MakeSubItemInTop<UI_GameBar>();
        gameBar.transform.localScale = Vector3.one;
        gameBar.transform.localPosition = new Vector3(0, 0, 0);

        // BGM
        Managers.Sound.Play("GameBGM", Define.Sound.BGM);
    }

    public override void Clear()
    {
        Debug.Log("Game Scene Clear!!!");
    }


}
