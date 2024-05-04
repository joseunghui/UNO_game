using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField] ItemSO itemSO;
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

        CardController cardController = gameObject.GetOrAddComponent<CardController>();
        cardController.SetItemSO(itemSO);
    }

    // 외부에서 사용하는 용도
    public GameMode.PVCMode GetGameMode()
    {
        return gameMode;
    }


    public override void Clear()
    {
        Debug.Log("Game Scene Clear!!!");
    }

    private void Update()
    {
        Managers.Input.OnUpdate();
    }


}
