using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadingVideo : UI_Scene
{

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(Define.LoadgingVideo));
    }
}
