using BackEnd;
using System.Collections;
using System.Collections.Generic;
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
            // 팝업 비활성화
            // SignInPopup.SetActive(false);
            // SignUpPopup.SetActive(false);
            // AccessGameBtn.SetActive(false);

            // BeforeGamePopup
            // BeforeGamePopup.SetActive(true);
        }
        else 
            yield break;
    }


    public override void Clear()
    {
        Debug.Log("Main Scene Clear!!!");
    }
}
