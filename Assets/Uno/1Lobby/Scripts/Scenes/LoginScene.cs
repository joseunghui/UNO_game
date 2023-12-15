using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{

    protected override void init()
    {
        base.init();

        ScenType = Define.Scene.Login; // here is Login Scene

        // 처음에는 회원가입
        Managers.UI.ShowPopup<UI_SignUp>();

        
    }

    public override void Clear()
    {
        Debug.Log("Login Scene Clear!!!");
    }
}
