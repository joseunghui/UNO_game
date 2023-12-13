using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : BaseScene
{

    protected override void init()
    {
        base.init();

        ScenType = Define.Scene.Loading; // here is Loadging Scene

        Managers.UI.MakeSubItem<UI_ProgressBar>();
    }


    public override void Clear()
    {
        
    }
}
