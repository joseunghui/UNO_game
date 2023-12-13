using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{

    protected override void init()
    {
        base.init();

        // here is Login Scene
        ScenType = Define.Scene.Login;

        // Managers.UI.ShowScene<UI_Inven>();
    }



    #region Sign In
    // Log-in 확인 버튼 클릭
    public void DoSignIn()
    {

    }
    #endregion
    #region Sign Up
    // Sign-Up 확인 버튼 클릭
    public void DoSignUp()
    {
        

    }
    #endregion



    public override void Clear()
    {
        Debug.Log("Login Scene Clear!!!");
    }
}
