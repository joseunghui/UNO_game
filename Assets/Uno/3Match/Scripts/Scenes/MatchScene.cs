using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        ScenType = Define.Scene.Match; // here is Match Scene

    }

    public override void Clear()
    {
        Debug.Log("Match Scene Clear!!!");
    }
}
