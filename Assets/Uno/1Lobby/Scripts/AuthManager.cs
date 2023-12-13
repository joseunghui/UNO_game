using UnityEngine;
using TMPro;
using BackEnd;
using System.Threading.Tasks; // 뒤끝

public class AuthManager : MonoBehaviour
{
    [Header("Main Canvas")]
    [SerializeField] private GameObject AccessGameBtn;
    [SerializeField] private GameObject AlertField;
    [SerializeField] private TextMeshProUGUI Alert;

    [Header("Sign In Popup")]
    [SerializeField] private GameObject SignInPopup;
    [SerializeField] private TextMeshProUGUI in_id_text;
    [SerializeField] private TMP_InputField in_pwd_text;

    [Header("Sign Up Popup")]
    [SerializeField] private GameObject SignUpPopup;
    [SerializeField] private TextMeshProUGUI up_id_text;
    [SerializeField] private TMP_InputField up_pwd_text;
    [SerializeField] private TMP_InputField up_pwd_conf_text;

    [Header("Before Game Popup")]
    [SerializeField] private GameObject BeforeGamePopup;

    private void Awake()
    {
        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
            // 기등록된 로컬 기기 자동 로그인
            BackendReturnObject autoLogin = Backend.BMember.LoginWithTheBackendToken();
            if (autoLogin.IsSuccess())
            {
                // 팝업 비활성화
                SignInPopup.SetActive(false);
                SignUpPopup.SetActive(false);
                AccessGameBtn.SetActive(false);

                // BeforeGamePopup
                BeforeGamePopup.SetActive(true);
            }
        }
        else
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
        // Test();
    }
    void Update()
    {
        Backend.AsyncPoll();
    }

    // 동기 함수를 비동기에서 호출하게 해주는 함수(유니티 UI 접근 불가)
    async void Test()
    {
        await Task.Run(() => {
            // BackendLogin.Instance.CustomSignUp("user1","1234"); // [추가] 뒤끝 회원가입(주석 처리)
            Login.Instance.CustomLogin("user1", "1234"); // [추가] 뒤끝 회원가입
            
            Debug.Log("테스트를 종료합니다.");
        });
    }




}