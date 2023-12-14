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

        GameObject canvas = GameObject.Find("LoadingScreen");

        Component compo = Managers.UI.MakeSubItem<UI_ProgressBar>(canvas.transform);
        compo.transform.localPosition = compo.transform.position;
    }


    public override void Clear()
    {
        
    }
}
