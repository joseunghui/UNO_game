using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    UserInfoData _data;
    Coroutine RechargeTimerCoroutine = null;
    int MAX_HEART = 5;
    int HeartRechargeInterval = 30;
    int remainTime;

    // 현재 씬 타입을 알아야 하는 일이 생기니까 
    // 객체 자체는 public 으로, Set은 protected로 선언해주기(자식 스크립트에서 접근 가능)
    public Define.Scene ScenType { get; protected set; } = Define.Scene.UnKnown; // 초기 설정 UnKnown

    void Awake()
    {
        Init();
    }

    public abstract void Clear();

    protected virtual void Init()
    {
        remainTime = 0;
        
        // EventSystem 없으면 생성하는 로직
        UnityEngine.Object obj = GameObject.FindObjectOfType(typeof(EventSystem));

        if (obj == null)
        {
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
        }
        // SetRechargeScheduler();
    }


    // 하트 사용
    void CheckIsFullHeart(Action onFinish = null)
    {
        if (RechargeTimerCoroutine == null)
            RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(HeartRechargeInterval));

        if (onFinish != null)
            onFinish();
    }

    // 하트 충전
    public void SetRechargeScheduler(Action onFinish = null)
    {
        _data = Managers.Data.GetUserInfoData();

        if (_data == null)
            return;

        if (RechargeTimerCoroutine != null)
        {
            StopCoroutine(RechargeTimerCoroutine);
        }

        var minDiff = (int)((DateTime.Now.ToLocalTime() - _data.heartChargeDt).TotalMinutes);
        var heartAdd = minDiff / HeartRechargeInterval;
        remainTime = minDiff % HeartRechargeInterval;

        _data.heart = _data.heart + heartAdd;

        if (_data.heart < MAX_HEART)
        {
            RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime, onFinish));
        }

        _data.heart = MAX_HEART;
        Managers.Data.UpdataUserData(Define.UpdateDateSort.UsingHeart, _data);

    }

    IEnumerator DoRechargeTimer(int _remainTime, Action onFinish = null)
    {
        if (_remainTime <= 0)
            _remainTime = HeartRechargeInterval;

        while (_remainTime > 0)
        {
            _remainTime -= 1;

            yield return new WaitForSeconds(2.0f);
        }

        _data.heart++;
        Managers.Data.SetUserInfoData(_data);
        Managers.Data.UpdataUserData(Define.UpdateDateSort.UsingHeart, _data);

        GameObject rankingPage = GameObject.Find(Define.UI_Scene.UI_Ranking.ToString());

        Managers.UI.ClosePopup(rankingPage.GetOrAddComponent<UI_Ranking>());
        Managers.UI.ShowPopup<UI_Ranking>();

        if (_data.heart >= MAX_HEART)
        {
            _data.heart = MAX_HEART;
            remainTime = 0;

            RechargeTimerCoroutine = null;
        }
        else
        {
            RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(HeartRechargeInterval, onFinish));
        }
    }
}