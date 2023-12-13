using BackEnd;
using System.Collections;
using UnityEngine;

public class MainScene : BaseScene
{
    protected override void init()
    {
        base.init();

        ScenType = Define.Scene.Main;

        StartCoroutine(BackendLoginExcu());
    }

    IEnumerator BackendLoginExcu()
    {
        // 기등록된 로컬 기기 자동 로그인
        BackendReturnObject autoLogin = Backend.BMember.LoginWithTheBackendToken();
        if (autoLogin.IsSuccess())
        {
            Managers.UI.ShowScene<UI_EnterGame>();
        }
        else 
            yield break;
    }


    public override void Clear()
    {
        Debug.Log("Main Scene Clear!!!");
    }
}
