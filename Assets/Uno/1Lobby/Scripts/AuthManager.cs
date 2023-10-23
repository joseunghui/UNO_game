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

        // 기존 유저 -> 로그인 / 신규 유저 -> 회원 가입
        if (UserDataIns.Instance.HavingThisUser(in_email_text.text))
        {
            // 기존유저 -> 그대로 로그인 후 게임 시작
            // level 선택 버튼 생성
            CreateLevelBtn();
        } else {
            // 신규유저
            Alert.text = "회원 정보가 없습니다. 회원 가입 후 이용해주세요.";
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