using UnityEngine;
using TMPro;
using GooglePlayGames; // PlayGamesPlatform 인스턴스를 활성화
using GooglePlayGames.BasicApi; // API 를 사용하기 위한 데이터를 초기화
using UnityEngine.SocialPlatforms;
using BackEnd; // 뒤끝

public class AuthManager : MonoBehaviour
{
    [Header("Main Canvas")]
    [SerializeField] public GameObject AlertField;
    [SerializeField] public TextMeshProUGUI Alert;
    // level button
    [SerializeField] public GameObject LevelBtnPrefab;
    [SerializeField] public GameObject LevelField;

    [Header("Sign In Popup")]
    [SerializeField] public GameObject SignInPopup;
    [SerializeField] public TextMeshProUGUI in_email_text;
    [SerializeField] public TMP_InputField in_pwd_text;

    [Header("Sign Up Popup")]
    [SerializeField] public GameObject SignUpPopup;
    [SerializeField] public TextMeshProUGUI up_email_text;
    [SerializeField] public TMP_InputField up_pwd_text;
    [SerializeField] public TMP_InputField up_pwd_conf_text;


    public void DoSignIn()
    {
        Debug.Log("email : " + in_email_text.text);
        Debug.Log("pwd : " + in_pwd_text.text);

        if (in_email_text.text == null)
        {
            Alert.text = "이메일을 입력해주세요.";
        }

        // 기존 유저 -> 로그인 / 신규 유저 -> 회원 가입
        // 여기서 HavingThisUser() 를 먼저 사용하려 했지만 로그인 전에는 뒤끝 접근이 불가능 했음
        if (Login.Instance.CustomLogin(in_email_text.text, in_pwd_text.text))
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

    public void DoSignUp()
    {
        Debug.Log("email : " + up_email_text.text);
        Debug.Log("pwd : " + up_pwd_text.text);

        if (up_email_text.text != null || up_email_text.text != "")
        {
            Alert.text = null;
        }

        if (up_email_text.text == null)
        {
            Alert.text = "이메일을 입력해주세요.";
        }

        // 비번 정규식 넣어서 수정해야함
        if (up_pwd_conf_text.text == up_pwd_text.text && up_pwd_text != null)
        {
            if (Login.Instance.CustomSignUp(up_email_text.text, up_pwd_text.text))
            {
                // Insert user Info data


                string alertTxt = "회원 가입이 완료 되었습니다.\n로그인 후 이용해주세요.";
                Alert.text = alertTxt.Replace("\\n", "\n");
                SignUpPopup.SetActive(false);
            }

        } else
        {
            Alert.text = "확인한 비밀번호가 일치하지 않습니다.";
        }

    }

    // 레벨 선택 버튼 생성
    void CreateLevelBtn()
    {
        LevelBtnPrefab = Resources.Load<GameObject>("LevelField");
        for (int i=0; i<3; i++)
        {
            GameObject button = Instantiate(LevelBtnPrefab);
            RectTransform btnPos = button.GetComponent<RectTransform>();
            button.transform.position = gameObject.transform.position;
        }
    }

    // Sign-up popup open
    void OpenSignUpPopup()
    {
        SignUpPopup.SetActive(true);
    }


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
    }
}