using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchScene : BaseScene
{
    [SerializeField] ItemSO itemSO;
    public GameMode.PVCMode gameMode = GameMode.PVCMode.None;

    protected override void Init()
    {
        base.Init();

        ScenType = Define.Scene.Match; // here is Match Scene

        UserInfoData data = Managers.Data.GetUserInfoData();

        // BGM
        Managers.Sound.Play("GameBGM", Define.Sound.BGM);

        UI_GameBar gameBar = Managers.UI.MakeSubItemInTop<UI_GameBar>();
        gameBar.transform.localScale = Vector3.one;
        gameBar.transform.localPosition = Vector3.zero;

        if (data != null)
        {
            gameBar.SetUserData(data.nickname, (data.freeDia + data.payDia).ToString());
            gameBar.SetUserHeartDate(data.heart);
        }

        UI_GameBar_Timer gameBarTimer = Managers.UI.MakeSubItemInTop<UI_GameBar_Timer>();
        gameBarTimer.transform.localScale = Vector3.one;
        gameBarTimer.transform.localPosition = new Vector3(-200f, -90f, 0);
        gameBarTimer.SetTimer("10");

        CardController cardController = gameObject.GetOrAddComponent<CardController>();
        cardController.SetItemSO(itemSO);
        cardController.SetStartCardCountbyGameMode(GameMode.PVCMode.None);


    }

    // 외부에서 사용하는 용도
    public GameMode.PVCMode GetGameMode()
    {
        return gameMode;
    }

    public override void Clear()
    {
        Debug.Log("Match Scene Clear!!!");
    }
}
