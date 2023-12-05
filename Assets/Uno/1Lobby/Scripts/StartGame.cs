using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject AlertMessage;

    public static int TurnlimitTime;                    // 레벨 별 턴 제한 시간
    public string keyStr = "havingHeart";               // 보유 하트 수 playerPrefs 해시키
    public int havingHeart;                             // 보유 하트 수 (DB)
    private DateTime GameQuitTime = new DateTime(1990, 1, 1).ToLocalTime();
    private const int MAX_HEART = 5;                    // 하트 최대값
    public int HeartRechargeInterval = 30;              // 하트 충전 간격(단위:분)
    private Coroutine RechargeTimerCoroutine = null;    // 하트 생성 틴
    private int RechargeRemainTime = 0;                 // 남은 시간 

    private void Awake()
    {
        // 유저의 하트 수 가져오기
        havingHeart = RankingDataManager.UserHeartCount;
        // 유저의 마지막 게임 접속 시간 가져오기 
        GameQuitTime = UserDataIns.Instance.GetUserLastUpdateTime();

        Debug.Log($"GameQuitTime >> {GameQuitTime}");
        Debug.Log($"Now Time >> {DateTime.Now}");

        // 하트 0개면 하트 채우는 로직에 필요한 데이터 세팅
        if (havingHeart == 0)
            Init();
    }
    public void Init()
    {
        havingHeart = 0;
        RechargeRemainTime = 0;
        // 유저의 마지막 게임 접속 시간 가져오기 
        GameQuitTime = UserDataIns.Instance.GetUserLastUpdateTime();
    }

    #region OnClickLevelSelectBtn()
    public void OnClickLevelSelectBtn(int mode)
    {
        if (havingHeart <= 0)
        {
            AlertMessage.SetActive(true);
            return;
        }
        else
        {
            TurnlimitTime = 20;
            if (mode == 1)
                TurnlimitTime = 10;
            else if (mode == 2)
                TurnlimitTime = 5;

            UseHeart();

            LoadingSceneManager.LoadScene("CardScenes");
        }
    }
    #endregion
 

    #region SetRechargeScheduler() 30분 경과할 때 마다 하트 충전
    public void SetRechargeScheduler(Action onFinish = null)
    {
        if (RechargeTimerCoroutine != null)
        {
            StopCoroutine(RechargeTimerCoroutine);
        }
        var minDiff = (int)((DateTime.Now.ToLocalTime() - GameQuitTime).TotalMinutes);
        Debug.Log("TimeDifference In Min :" + minDiff + "m");

        var heartToAdd = minDiff / HeartRechargeInterval; // 30분 마다 1개
        var remainTime = minDiff % HeartRechargeInterval; // 잔여 시간

        havingHeart += heartToAdd;
        if (havingHeart >= MAX_HEART)
        {
            havingHeart = MAX_HEART; // 5개가 최대
        }
        else
        {
            RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime, onFinish));
        }
    }
    #endregion
    #region DoRechargeTimer() 
    private IEnumerator DoRechargeTimer(int remainTime, Action onFinish = null)
    {
        if (remainTime <= 0)
        {
            RechargeRemainTime = HeartRechargeInterval;
        }
        else
        {
            RechargeRemainTime = remainTime;
        }

        while (RechargeRemainTime > 0)
        {
            RechargeRemainTime -= 1;
            Debug.Log($"RechargeRemainTime >> {RechargeRemainTime}");
            yield return new WaitForSeconds(60.0f); // 60s = 1m
        }

        havingHeart++;

        if (havingHeart >= MAX_HEART)
        {
            havingHeart = MAX_HEART;
            RechargeRemainTime = 0;

            RechargeTimerCoroutine = null;
        }
        else
        {
            RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(HeartRechargeInterval, onFinish));
        }
    }
    #endregion
    #region UseHeart() 하트 사용 시 PlayerPrefs의 havingHeart 차감
    public void UseHeart(Action onFinish = null)
    {
        if (havingHeart <= 0)
            return;

        havingHeart--;
        PlayerPrefs.SetInt(keyStr, havingHeart);
        PlayerPrefs.Save();

        if (RechargeTimerCoroutine == null)
        {
            RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(HeartRechargeInterval));
        }
        if (onFinish != null)
        {
            onFinish();
        }
    }
    #endregion
    #region SaveUserHeartData() 게임 종료 전에 하트 수 디비에 저장
    private IEnumerator SaveUserHeartData()
    {
        if (havingHeart.ToString() == string.Empty)
            yield break;

        if (PlayerPrefs.HasKey(keyStr))
            UserDataIns.Instance.UserHeartDataUpdate(PlayerPrefs.GetInt(keyStr));
        else
            UserDataIns.Instance.UserHeartDataUpdate(havingHeart);
    }
    #endregion
    #region 게임 중단, 이탈, 종료, 복귀
    //게임 초기화, 중간 이탈, 중간 복귀 시 실행되는 함수
    public void OnApplicationFocus(bool value)
    {
        if (value)
            SetRechargeScheduler();

        StartCoroutine(SaveUserHeartData()); // 종료할때 마다 실제 디비에 하트 수 저장
    }
    //게임 종료 시 실행되는 함수
    public void OnApplicationQuit()
    {
        StartCoroutine(SaveUserHeartData()); // 종료할때 마다 실제 디비에 하트 수 저장
    }
    #endregion
}
