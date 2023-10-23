using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 뒤끝 SDK namespace 추가
using BackEnd;

public class Login
{
    private static Login _instance = null;

    public static Login Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Login();
            }

            return _instance;
        }
    }

    public void CustomSignUp(string id, string pw, string nick)
    {
        Debug.Log("회원가입을 요청합니다.");

        var bro = Backend.BMember.CustomSignUp(id, pw, nick);

        if (bro.IsSuccess())
        {
            // userInfo 테이블에 유저 정보 insert
            // UserDataIns.Instance.InsertUserData();
            Debug.Log("회원가입에 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("회원가입에 실패했습니다. : " + bro);
        }
    }

    public void CustomLogin(string id, string pw)
    {
        Debug.Log("로그인을 요청합니다.");

        var bro = Backend.BMember.CustomLogin(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("로그인이 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("로그인이 실패했습니다. : " + bro);
        }
    }

}
