using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using GooglePlayGames; // PlayGamesPlatform 인스턴스를 활성화
using GooglePlayGames.BasicApi; // API 를 사용하기 위한 데이터를 초기화
using UnityEngine.SocialPlatforms;



public class AuthManager : MonoBehaviour
{
    // bool bWait = false;
    public TMP_Text text;

    private void Awake()
    {
        // 디버그용 함수
        PlayGamesPlatform.DebugLogEnabled = true;

        // 구글 관련 서비스 활성화
        PlayGamesPlatform.Activate();


        if (Social.localUser.authenticated) // local에 연결된 계정이 있는지 확인 -> 게임으로 이동!
        {
            SceneManager.LoadScene(1); // 게임화면 이동
        }
        else AccessGame(); // 없으면 계정 인증 시작하기
    }

    public void AccessGame()
    {
        // 로그인 단계 : local에 연결된 계정이 있는지 확인 -> 안되었다면, 인증 단계 시작!
        if (!Social.localUser.authenticated)
        {
            // 계정 인증
            Social.localUser.Authenticate((bool isSuccess) =>
            {

                if (isSuccess)
                {

                    Debug.Log("Login Success!");
                    Debug.Log("Success : " + Social.localUser.userName);
                    text.text = Social.localUser.userName + "님 환영합니다!";
                }
                else
                {
                    Debug.Log("Login Fail!");
                    text.text = "로그인에 실패하였습니다.";
                }
            });
        }
    }

}