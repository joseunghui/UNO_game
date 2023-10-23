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
    [Header("Sign In Popup")]
    [SerializeField] public GameObject SignInPopup;

    [Header("Sign Up Popup")]
    [SerializeField] public GameObject SignUpPopup;
}