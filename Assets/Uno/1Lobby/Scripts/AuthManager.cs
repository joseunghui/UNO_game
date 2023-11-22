using UnityEngine;
using TMPro;
using BackEnd; // 뒤끝

public class AuthManager : MonoBehaviour
{
    [Header("Main Canvas")]
    [SerializeField] public GameObject AccessGameBtn;
    [SerializeField] public GameObject AlertField;
    [SerializeField] public TextMeshProUGUI Alert;

    [Header("Sign In Popup")]
    [SerializeField] public GameObject SignInPopup;
    [SerializeField] public TextMeshProUGUI in_id_text;
    [SerializeField] public TMP_InputField in_pwd_text;

    [Header("Sign Up Popup")]
    [SerializeField] public GameObject SignUpPopup;
    [SerializeField] public TextMeshProUGUI up_id_text;
    [SerializeField] public TMP_InputField up_pwd_text;
    [SerializeField] public TMP_InputField up_pwd_conf_text;

    [Header("Before Game Popup")]
    [SerializeField] public GameObject BeforeGamePopup;

    [Header("Level Select Popup")]
    [SerializeField] public GameObject LevelSelectPopup;
    [SerializeField] public GameObject LevelField;

    #region Sign In
    // Log-in 확인 버튼 클릭
    public void DoSignIn()
    {

        if (in_id_text.text == null)
        {
            Alert.text = "아이디를 입력해주세요.";
        }

        // 기존 유저 -> 로그인 / 신규 유저 -> 회원 가입
        if (Login.Instance.CustomLogin(in_id_text.text, in_pwd_text.text))
        {
            // 기존유저 -> 그대로 로그인 후 게임 시작
            // level 선택 버튼 생성
            CreateLevelBtn();
        } else {
            // 신규유저 등록 필요 -> 회원가입 유도
            Alert.text = "회원 정보가 없습니다. 회원 가입 후 이용해주세요.";
            Invoke("OpenSignUpPopup", 0.5f);
 
        }
    }
    #endregion
    #region Sign Up
    // Sign-Up 확인 버튼 클릭
    public void DoSignUp()
    {
        if (up_id_text.text != null || up_id_text.text != "")
        {
            Alert.text = null;
        }

        if (up_id_text.text == null)
        {
            Alert.text = "아이디를 입력해주세요.";
        }

        // 비번 정규식 넣어서 수정해야함
        if (up_pwd_conf_text.text == up_pwd_text.text && up_pwd_text != null)
        {
            if (Login.Instance.CustomSignUp(up_id_text.text, up_pwd_text.text))
            {
                UserDataIns.Instance.InsertUserData();

                string alertTxt = "회원 가입이 완료 되었습니다.\n로그인 후 이용해주세요.";
                Alert.text = alertTxt.Replace("\\n", "\n");
                SignUpPopup.SetActive(false);
            }

        } else
        {
            Alert.text = "확인한 비밀번호가 일치하지 않습니다.";
        }

    }
    #endregion

    void Start()
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
                Debug.Log("자동 로그인에 성공했습니다.");

                CreateBeforeGamePopup();
            }
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생 
        }
    }

    void CreateBeforeGamePopup()
    {
        // 팝업 비활성화
        SignInPopup.SetActive(false);
        SignUpPopup.SetActive(false);
        AccessGameBtn.SetActive(false);

        // BeforeGamePopup
        BeforeGamePopup.SetActive(true);
    }

    // 레벨 선택 버튼 생성
    public void CreateLevelBtn()
    {
        Alert.text = "환영합니다.";
        // 팝업 비활성화
        SignInPopup.SetActive(false);
        SignUpPopup.SetActive(false);
        AccessGameBtn.SetActive(false);
        BeforeGamePopup.SetActive(false);

        // 레벨선택 필드 활성화
        LevelSelectPopup.SetActive(true);
    }


    // Sign-up popup open
    void OpenSignUpPopup()
    {
        SignUpPopup.SetActive(true);
    }

}