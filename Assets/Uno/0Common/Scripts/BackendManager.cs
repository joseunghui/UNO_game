using UnityEngine;
using System.Threading.Tasks; // [변경] async 기능을 이용하기 위해서는 해당 namepsace가 필요합니다.

// 뒤끝 SDK namespace 추가
using BackEnd;

public class BackendManager : MonoBehaviour
{
    void Start()
    {
        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생 
        }

        Test();
    }

    // =======================================================
    // [추가] 동기 함수를 비동기에서 호출하게 해주는 함수(유니티 UI 접근 불가)
    // =======================================================
    async void Test()
    {
        await Task.Run(() => {
            // Login.cs => CustomSignUp() Test
            Login.Instance.CustomSignUp("sample", "1234"); // 회원가입 테스트
            UserDataIns.Instance.InsertUserData();
            // Login.Instance.CustomLogin("user1", "1234"); // 로그인 테스트

            Debug.Log("테스트를 종료합니다.");
        });
    }
}
