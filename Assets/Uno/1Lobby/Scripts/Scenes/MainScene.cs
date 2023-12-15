using BackEnd;
using System.Collections;
using UnityEngine;

public class MainScene : BaseScene
{
    protected override void init()
    {
        base.init();

        ScenType = Define.Scene.Main;
        // BGM
        Managers.Sound.Play("BGM", Define.Sound.BGM);
        Managers.UI.MakeSubItemInOldCanvas<UI_EnterGame>();
    }


    public override void Clear()
    {
        Debug.Log("Main Scene Clear!!!");
    }
}
