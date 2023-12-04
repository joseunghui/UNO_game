using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
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
        havingHeart = RankingDataManager.UserHeartCount;
        Debug.Log($"유저의 보유 하트 : {havingHeart}");

        if (havingHeart <= 0)
            Init();
       
    }

    //게임 초기화, 중간 이탈, 중간 복귀 시 실행되는 함수
    public void OnApplicationFocus(bool value)
    {
        LoadHeartInfo();
        LoadAppQuitTime();

        if (value)
            SetRechargeScheduler();

        StartCoroutine(SaveUserHeartData()); // 종료할때 마다 실제 디비에 하트 수 저장
    }
    //게임 종료 시 실행되는 함수
    public void OnApplicationQuit()
    {
        SaveHeartInfo();
        SaveAppQuitTime();
        StartCoroutine(SaveUserHeartData()); // 종료할때 마다 실제 디비에 하트 수 저장
    }

    public void Init()
    {
        havingHeart = 0;
        RechargeRemainTime = 0;
        GameQuitTime = new DateTime(1990, 1, 1).ToLocalTime();
    }

    #region OnClickLevelSelectBtn()
    public void OnClickLevelSelectBtn(int mode)
    {
        TurnlimitTime = 20;
        if (mode == 1)
            TurnlimitTime = 10;
        else if (mode == 2)
            TurnlimitTime = 5;

        UseHeart();

        LoadingSceneManager.LoadScene("CardScenes");
    }
    #endregion
    #region LoadHeartInfo() 유저의 하트 수 playerPrefs 에서 가져오기
    public bool LoadHeartInfo()
    {
        Debug.Log("LoadHeartInfo() start");
        bool result = false;

        try
        {
            if (PlayerPrefs.HasKey(keyStr))
            {
                Debug.Log("PlayerPrefs has key : havingHeart");
                havingHeart = PlayerPrefs.GetInt(keyStr);
                if (havingHeart < 0)
                    havingHeart = 0;
            }
            else
            {
                havingHeart = MAX_HEART;
            }
            Debug.Log($"Loaded having heart >> {havingHeart}");
        } catch (Exception e)
        {
            Debug.LogError($"Load heart info failed : {e}");
        }
        return result;
    }
    #endregion
    #region LoadAppQuitTime() 유저가 게임 종료한 시간 가져오기
    public bool LoadAppQuitTime() 
    {
        Debug.Log("LoadAppQuitTime");
        bool result = false;

        try
        {
            if (PlayerPrefs.HasKey("AppQuitTime")) 
            {
                Debug.Log("PlayerPrefs ha key : AppQuitTime");

                var appQuitTime = string.Empty;

                appQuitTime = PlayerPrefs.GetString("AppQuitTime");
                GameQuitTime = DateTime.FromBinary(Convert.ToInt64(appQuitTime));

            }
            result = true;
            Debug.Log($"Loaded application time : {GameQuitTime.ToString()}");
        } catch (Exception e)
        {
            Debug.Log($"Load application quit time failed : {e}");
        }
        return result;
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
        Debug.Log("Heart to add : " + heartToAdd);

        var remainTime = minDiff % HeartRechargeInterval; // 잔여 시간
        Debug.Log("RemainTime : " + remainTime);

        havingHeart += heartToAdd;
        if (havingHeart >= MAX_HEART)
        {
            havingHeart = MAX_HEART; // 5개가 최대
        }
        else
        {
            RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime, onFinish));
        }
        Debug.Log("finish charge >> havingHeart : " + havingHeart);
    }
    #endregion
    #region SaveHeartInfo() PlayerPrefs 에 유저 하트 수 저장
    public bool SaveHeartInfo()
    {
        Debug.Log("SaveHeartInfo");
        bool result = false;

        try
        {
            PlayerPrefs.SetInt(keyStr, havingHeart);
            PlayerPrefs.Save();

            Debug.Log("Saved havingHeart : " + havingHeart);
            result = true;
        }
        catch (Exception e)
        {
            Debug.LogError("SaveHeartInfo Failed (" + e.Message + ")");
        }

        return result;
    }
    #endregion
    #region SaveAppQuitTime() 
    public bool SaveAppQuitTime()
    {
        Debug.Log("SaveAppQuitTime");

        bool result = false;
        try
        {
            var appQuitTime = DateTime.Now.ToLocalTime().ToBinary().ToString();

            PlayerPrefs.SetString("AppQuitTime", appQuitTime);
            PlayerPrefs.Save();

            Debug.Log("Saved AppQuitTime : " + DateTime.Now.ToLocalTime().ToString());
            result = true;
        }
        catch (Exception e)
        {
            Debug.LogError("SaveAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }
    #endregion
    #region UseHeart() 하트 사용 시 PlayerPrefs의 havingHeart 차감
    public void UseHeart(Action onFinish = null)
    {
        if (havingHeart <= 0)
            return;

        havingHeart--;
        
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
    #region DoRechargeTimer() 
    private IEnumerator DoRechargeTimer(int remainTime, Action onFinish = null)
    {
        Debug.Log("DoRechargeTimer");

        if (remainTime <= 0)
        {
            RechargeRemainTime = HeartRechargeInterval;
        }
        else
        {
            RechargeRemainTime = remainTime;
        }
        Debug.Log("heartRechargeTimer : " + RechargeRemainTime + "m");

        while (RechargeRemainTime > 0)
        {
            Debug.Log("heartRechargeTimer : " + RechargeRemainTime + "m");

            RechargeRemainTime -= 1;
            yield return new WaitForSeconds(1f);
        }

        havingHeart++;

        if (havingHeart >= MAX_HEART)
        {
            havingHeart = MAX_HEART;
            RechargeRemainTime = 0;

            Debug.Log("havingHeart reached max amount");
            RechargeTimerCoroutine = null;
        }
        else
        {
            RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(HeartRechargeInterval, onFinish));
        }
        Debug.Log("havingHeart : " + havingHeart);
    }
    #endregion
    #region SaveUserHeartData() 게임 종료 전에 하트 수 디비에 저장
    private IEnumerator SaveUserHeartData()
    {
        yield return new WaitForSeconds(0.1f);
        UserDataIns.Instance.UserHeartDataUpdate(PlayerPrefs.GetInt(keyStr));
    }
    #endregion
}
