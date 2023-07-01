using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames; // PlayGamesPlatform 인스턴스를 활성화
using GooglePlayGames.BasicApi; // API 를 사용하기 위한 데이터를 초기화

public class AuthManager : MonoBehaviour
{
    bool bWait = false;
    // public Text text;

    private void Awake()
    {
        // 객체 초기화 
        // 초기화 함수, 인스턴스를 만드는 역할
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());

        // 디버그용 함수
        PlayGamesPlatform.DebugLogEnabled = true;

        // 구글 관련 서비스 활성화
        PlayGamesPlatform.Activate();

        // 로그인 여부 띄우기
        // text.text = "no login";
    }

    public void loginUserBtn()
    {
        // 로그인 단계 : local에 연결된 계정이 있는지 확인 -> 안되었다면, 인증 단계 시작!
        if (!Social.localUser.authenticated)
        {
            // 계정 인증
            Social.localUser.Authenticate((bool isSuccess) => 
            {
                if(isSuccess) 
                {
                    Debug.Log("Login Success!");
                    Debug.Log("Success : " + Social.localUser.userName);
                    // text.text = Social.localUser.userName;
                }
                else 
                {
                    Debug.Log("Login Fail!");
                    // text.text = "Fail";
                }
            });
        }
    }

    public void logOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        // text.text = "LogOut";
    }

}