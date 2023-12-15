using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadingVideo : UI_Scene
{

    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();

        Bind<GameObject>(typeof(Define.LoadgingVideo));
    }
}
