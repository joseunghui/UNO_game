using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 뒤끝 SDK namespace 추가
using BackEnd;
using System;
using Random = System.Random;

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

    // result 를 받아야 함
    public bool CustomSignUp(string id, string pw)
    {
        Debug.Log("회원가입을 요청합니다.");

        var bro = Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            // 닉네임 자동 생성
            string randomNick = GetRandomDigit(6);
            Backend.BMember.CreateNickname(randomNick, ( callback ) =>
            {
                Debug.Log("회원가입에 성공했습니다. : " + bro);
            });
            return true;
        }
        else
        {
            Debug.LogError("회원가입에 실패했습니다. : " + bro);
            return false;
        }
    }

    // result 를 받아야 함
    public bool CustomLogin(string id, string pw)
    {
        Debug.Log("로그인을 요청합니다.");

        var bro = Backend.BMember.CustomLogin(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("로그인이 성공했습니다. : " + bro);
            return true;
        }
        else
        {
            Debug.LogError("로그인이 실패했습니다. : " + bro);
            return false;
        }
    }


    // 난수 사용 최초 닉네임 설정
    public static string GetRandomDigit(int length)
    {
        string s = "user_";
        Random r = new Random((int)DateTime.Now.Ticks);

        //Random r = new Random(Convert.ToInt32(DateTime.Now.ToString("fffmmss")));
        int[] Random = new int[length];

        for (int i = 0; i < length; i++)
        {
            Random[i] = (int)r.Next(0, 10); //0보다 크거나 같고, 10보다 작은 
        }

        for (int i = 0; i < length; i++)
        {
            s += Random[i].ToString();
        }
        return s;
    }

}
