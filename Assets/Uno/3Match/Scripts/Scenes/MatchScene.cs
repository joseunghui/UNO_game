using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchScene : BaseScene
{
    protected override void init()
    {
        base.init();

        ScenType = Define.Scene.Match; // here is Match Scene

    }

    public override void Clear()
    {
        Debug.Log("Match Scene Clear!!!");
    }
}
