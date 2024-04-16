using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        ScenType = Define.Scene.Match; // here is Match Scene

        UserInfoData data = Managers.Data.GetUserInfoData();

        // BGM
        Managers.Sound.Play("GameBGM", Define.Sound.BGM);

        Debug.Log($"this is Match Scene");

        Managers.Data.Match.AccessMatchServer();

    }

    public override void Clear()
    {
        Debug.Log("Match Scene Clear!!!");
    }
}
