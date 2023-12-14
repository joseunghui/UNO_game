using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : BaseScene
{

    protected override void init()
    {
        base.init();

        ScenType = Define.Scene.Loading; // here is Loadging Scene

        Debug.Log("this is LoadingScene");
        Debug.Log($" ");

        Transform parent = GameObject.Find("Background").transform;
        Managers.UI.MakeSubItem<UI_ProgressBar>(parent);
    }


    public override void Clear()
    {
        
    }
}
