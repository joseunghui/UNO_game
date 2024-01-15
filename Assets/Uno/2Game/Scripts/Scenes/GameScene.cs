using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameScene : BaseScene
{
    public GameMode.PVCMode gameMode = GameMode.PVCMode.None;

    protected override void Init()
    {
        base.Init();

        UserInfoData data = Managers.Data.GetUserInfoData();

        ScenType = Define.Scene.Game; // here is Game Scene

        Managers.UI.ShowPopup<UI_LevelSelect>();

        UI_GameBar gameBar = Managers.UI.MakeSubItemInTop<UI_GameBar>();
        gameBar.transform.localScale = Vector3.one;
        gameBar.transform.localPosition = Vector3.zero;
        
        if (data != null )
        {
            gameBar.SetUserData(data.nickname, (data.freeDia + data.payDia).ToString());
            gameBar.SetUserHeartDate(data.heart);
        }

        // BGM
        Managers.Sound.Play("GameBGM", Define.Sound.BGM);

        // Game Buttons
        UI_Turn turnButtons = Managers.UI.MakeSubItemInContent<UI_Turn>();
        turnButtons.transform.localScale = Vector3.one;

        gameObject.GetOrAddComponent<CardController>();
        gameObject.GetOrAddComponent<TurnController>();
        gameObject.GetOrAddComponent<OrderController>();

    }

    // �ܺο��� ����ϴ� �뵵
    public GameMode.PVCMode GetGameMode()
    {
        return gameMode;
    }


    public override void Clear()
    {
        Debug.Log("Game Scene Clear!!!");
    }


}
