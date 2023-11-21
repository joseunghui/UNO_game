using System;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System.ComponentModel;

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

    [Header("Status Bar")]
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
    public float timeLimit;
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
        timeLimit -= Time.deltaTime;
        TimerTxt.text = Math.Round(timeLimit, 3).ToString("#.##");
    }
    #endregion
    #region Set Data 
    private IEnumerator SetDataConnc()
    {
        yield return data = UserDataIns.Instance.GetMyAllData();

        nicknameTxt.text = data.nickname;
        diaTxt.text = (data.payDia + data.freeDia).ToString();

        StartCoroutine(SetModeValue());
        StartCoroutine(SetGradeIconImage());
        StartCoroutine(SetHeartIconList());
    }
    #endregion
    #region Mode 
    IEnumerator SetModeValue()
    {
        yield return null;

        if (timeLimit == 0)
            yield break;

        // mode
        if (timeLimit == 20)
        {
            modeTxt.text = "EASY";
        }
        else if (timeLimit == 10)
        {
            modeTxt.text = "NAR";
        }
        else if (timeLimit == 5)
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
}
