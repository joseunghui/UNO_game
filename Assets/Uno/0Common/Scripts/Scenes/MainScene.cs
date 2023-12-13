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
        // ���ϵ� ���� ��� �ڵ� �α���
        BackendReturnObject autoLogin = Backend.BMember.LoginWithTheBackendToken();
        if (autoLogin.IsSuccess())
        {
            // �˾� ��Ȱ��ȭ
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
