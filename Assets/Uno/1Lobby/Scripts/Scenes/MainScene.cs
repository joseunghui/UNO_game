using BackEnd;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MainScene : BaseScene
{
    UI_EnterGame ui_EnterGame;
    protected override void Init()
    {
        base.Init();

        ScenType = Define.Scene.Main;
        
        // BGM
        Managers.Sound.Play("BGM", Define.Sound.BGM);
        
        // Game Start Button
        ui_EnterGame = Managers.UI.MakeSubItemInOldCanvas<UI_EnterGame>();
        ui_EnterGame.transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        bool Excu = false;
        if (ui_EnterGame.IsDestroyed() && Excu == false)
        {
            // SetRechargeScheduler();
            Excu = true;
        }
    }


    public override void Clear()
    {
        Debug.Log("Main Scene Clear!!!");
    }
}
