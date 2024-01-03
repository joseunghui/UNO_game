using BackEnd;
using System.Collections;
using UnityEngine;

public class MainScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        ScenType = Define.Scene.Main;
        // BGM
        Managers.Sound.Play("BGM", Define.Sound.BGM);
        UI_EnterGame ui_EnterGame = Managers.UI.MakeSubItemInOldCanvas<UI_EnterGame>();
        ui_EnterGame.transform.localPosition = Vector3.zero;
    }

    public override void Clear()
    {
        Debug.Log("Main Scene Clear!!!");
    }
}
