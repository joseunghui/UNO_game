using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using GooglePlayGames; // PlayGamesPlatform 인스턴스를 활성화
using GooglePlayGames.BasicApi; // API 를 사용하기 위한 데이터를 초기화
using UnityEngine.SocialPlatforms;
using BackEnd; // 뒤끝

public class AuthManager : MonoBehaviour
{
    [Header("Sign In Popup")]
    [SerializeField] public GameObject SignInPopup;
    [SerializeField] public TextMeshProUGUI in_email_text;
    [SerializeField] public TextMeshProUGUI in_pwd_text;

    [Header("Sign Up Popup")]
    [SerializeField] public GameObject SignUpPopup;
    [SerializeField] public TextMeshProUGUI up_email_text;
    [SerializeField] public TextMeshProUGUI up_pwd_text;
    [SerializeField] public TextMeshProUGUI up_pwd_conf_text;

    void Start()
    {
        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success

            // 기존 유저 -> 로그인 / 신규 유저 -> 회원 가입
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생 
        }
    }
}