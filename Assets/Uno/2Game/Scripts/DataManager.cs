using System;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    private static DataManager instance = null;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }

    [Header("Option Popup")]
    [SerializeField] private GameObject OptionPopup;

    [Header("Game Scene")]
    [SerializeField] private TextMeshProUGUI modeTxt;
    [SerializeField] private TextMeshProUGUI TimerTxt;
    [SerializeField] private Image GradeIcon;
    [SerializeField] private TextMeshProUGUI nicknameTxt;
    [SerializeField] private TextMeshProUGUI diaTxt;
    [SerializeField] private GameObject HeartGroup;
    [SerializeField] private GameObject Heart;

    public Sprite Sliver;
    public Sprite Gold;

    UserInfoData data;
    public bool IsMyTurn;
    private float timeLimit;
    private int mode;
    private bool AddCardAfterTimeOut;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        timeLimit = (float)StartGame.TurnlimitTime;

        if (timeLimit == 20)
            mode = 0;
        if (timeLimit == 10)
            mode = 1;
        if (timeLimit == 5)
            mode = 2;
        IsMyTurn = false;
        StartCoroutine(SetDataConnc());
    }

    private void Update()
    {
        if (timeLimit > 0)
        {
            AddCardAfterTimeOut = false;
            if (IsMyTurn)
                StartCoroutine(StartTimer());
        } else
        {
            // 시간 지나면 카드먹고 턴 넘기기
            IsMyTurn = false;
            if (!AddCardAfterTimeOut)
            {
                StartCoroutine(AfterTimeoutExc());
                AddCardAfterTimeOut = true;
            }
                
        }
    }

    #region after time out
    IEnumerator AfterTimeoutExc()
    {
        yield return null;
        CardManager.instance.AddCard(true);

        yield return CardManager.instance.TryPutCard(false);
        timeLimit = (float)StartGame.TurnlimitTime;

    }
    #endregion
    #region Timer Running
    IEnumerator StartTimer()
    {
        yield return null;

        if (OptionPopup.activeSelf == true)
        {
            yield return new WaitUntil( () => OptionPopup.activeSelf == false);
        }

        timeLimit -= Time.deltaTime;
        TimerTxt.text = Math.Round(timeLimit, 3).ToString("#.##");
    }
    #endregion
    #region Set Data 
    private IEnumerator SetDataConnc()
    {
        yield return data = UserDataIns.Instance.GetMyAllData();

        StartCoroutine(SetNicknameValue());
        StartCoroutine(SetModeValue());
        StartCoroutine(SetGradeIconImage());
        StartCoroutine(SetHeartIconList());
    }
    #endregion

    #region nickname & dia
    IEnumerator SetNicknameValue()
    {
        yield return null;

        if (data.nickname == null)
            yield break;

        nicknameTxt.text = data.nickname;
        diaTxt.text = (data.payDia + data.freeDia).ToString();
    }
    #endregion
    #region Mode 
    IEnumerator SetModeValue()
    {
        yield return null;

        if (timeLimit == 0)
            yield break;

        // mode
        if (mode == 0)
        {
            modeTxt.text = "EASY";
        }
        else if (mode == 1)
        {
            modeTxt.text = "NAR";
        }
        else if (mode == 2)
        {
            modeTxt.text = "HARD";
        }
    }
    #endregion
    #region Grade
    IEnumerator SetGradeIconImage()
    {
        yield return null;

        if (GradeIcon.GetComponent<Image>().sprite == null)
            yield break;

        // Grade
        if (data.grade == 1)
        {
            GradeIcon.GetComponent<Image>().sprite = Sliver;
        }
        else if (data.grade == 2)
        {
            GradeIcon.GetComponent<Image>().sprite = Gold;
        }
    }
    #endregion
    #region Heart
    IEnumerator SetHeartIconList()
    {
        yield return null;

        if (data.heart == 0)
        {
            // 하트 없기 때문에 로비로 이동
            LoadingSceneManager.LoadScene("MainScenes");
        }

        Debug.Log($"Heart : {data.heart}");
        // heart Icon Image
        for (int i = 0; i < data.heart; i++)
        {
            GameObject heartIcon = Instantiate(Heart, HeartGroup.transform.position, Quaternion.identity);
            heartIcon.transform.localScale = new Vector3(0.0065f, 0.0065f, 0.0f);
            heartIcon.transform.SetParent(HeartGroup.transform);
        }
    }
    #endregion

    public void ResetThisGame()
    {
        // 게임 모드를 동일하게 유지하고 다시 실행해야함
        Debug.Log($"게임 리셋 => 현재 모드 {mode}");
    }

    public void BackToLobbyBtnClick()
    {
        Destroy(TurnManager.instance);
        Destroy(CardManager.instance);
        Destroy(EntityManager.instance);

        LoadingSceneManager.LoadScene("MainScenes");
        // 이렇게 돌아가서 다시 레벨 선택 하면 카드 배포가 안됨(TurnManager 안돌아감)
    }
}
