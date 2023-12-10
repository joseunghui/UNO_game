using System;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class DataManager : StartGame
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

    private UserInfoData data;
    public bool IsMyTurn;
    private float timeLimit;
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
        timeLimit = (float)TurnlimitTime;

        StartCoroutine(SetDataConnc());
    }

    private void Update()
    {
        if (timeLimit > 0)
        {
            AddCardAfterTimeOut = false;
            if (IsMyTurn)
                StartCoroutine(StartTimer());
            else
                timeLimit = (float)TurnlimitTime;
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

        timeLimit = (float)TurnlimitTime;
    }
    #endregion
    #region Timer Running
    public IEnumerator StartTimer()
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
        UserDataIns.Instance.GetMyAllData();
        data = new UserInfoData();
        data.nickname = UserDataIns.userInfo.nickname;
        data.heart = UserDataIns.userInfo.heart;
        data.grade = UserDataIns.userInfo.grade;
        data.freeDia = UserDataIns.userInfo.freeDia;
        data.payDia = UserDataIns.userInfo.payDia;
        data.totalCnt = UserDataIns.userInfo.totalCnt;
        data.winCnt = UserDataIns.userInfo.winCnt;

        if (data == null)
            yield break;

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
        if (timeLimit == 0)
            yield break;

        switch(mode)
        {
            case Mode.Easy:
                modeTxt.text = "EASY";
                break;
            case Mode.Normal:
                modeTxt.text = "NOR";
                break;
            case Mode.Hard:
                modeTxt.text = "HARD";
                break;
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
        int modeInt = 0;

        if (mode == Mode.Normal)
            modeInt = 1;
        else if (mode == Mode.Hard)
            modeInt = 2;

        OnClickLevelSelectBtn(modeInt);
    }

    public void BackToLobbyBtnClick()
    {
        LoadingSceneManager.LoadScene("MainScenes");
    }
}
