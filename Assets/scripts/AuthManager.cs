using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using TMPro; 

public class AuthManager : MonoBehaviour
{
    [SerializeField] TMP_InputField emailField;
    [SerializeField] TMP_InputField pwField;

    // 인증을 관리할 객체 선언
    Firebase.Auth.FirebaseAuth auth;

    void Awake()
    {
        // 객체 초기화 
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }
    public void loginUserBtn()
    {
        // 제공되는 함수 : email 과 pw로 로그인
        auth.SignInWithEmailAndPasswordAsync(emailField.text, pwField.text).ContinueWith(
            task => {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    Debug.Log(emailField.text + "님 환영합니다!");
                }
                else 
                {
                    Debug.Log("아이디와 비밀번호를 확인해주세요.");
                }
            }
        );
    }
    public void addUserBtn() {
        // 제공 되는 함수 : 이메일과 비번으로 회원가입
        auth.CreateUserWithEmailAndPasswordAsync(emailField.text, pwField.text).ContinueWith(
            task => {
                if (!task.IsCanceled && !task.IsFaulted) 
                {
                    Debug.Log(emailField.text + "로 회원가입 완료");
                }
                else
                {
                    Debug.Log("회원가입 실패");
                }
            }
        );
    }
}
