using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public UserInfoData userInfoData { get; private set; } = new UserInfoData();
    public List<Ranking> rankingDatas { get; private set; } = new List<Ranking>();

    public void init()
    {
        var bro = Backend.Initialize(true); // �ڳ� �ʱ�ȭ

        // �ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            Debug.Log("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 204 Success
        }
        else
        {
            Debug.LogError("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 400�� ���� �߻�
        }
    }

    private void Update()
    {
        Backend.AsyncPoll();
    }

    public void InsertUserData()
    {
        // Game Data Insert
        UserDataIns.Instance.InsertUserData();
        RankingData.Instance.InsertRanking();
    }

    public void SelectUserData()
    {
        UserDataIns.Instance.GetMyAllData();

        userInfoData.nickname = UserDataIns.userInfo.nickname;
        userInfoData.nickChange = UserDataIns.userInfo.nickChange;
        userInfoData.heart = UserDataIns.userInfo.heart;
        userInfoData.grade = UserDataIns.userInfo.grade;
        userInfoData.freeDia = UserDataIns.userInfo.freeDia;
        userInfoData.payDia = UserDataIns.userInfo.payDia;
        userInfoData.totalCnt = UserDataIns.userInfo.totalCnt;
        userInfoData.winCnt = UserDataIns.userInfo.winCnt;

        RankingData.Instance.RankingGet();
        
        for (int i=0; i < RankingData.ranks.Count; i++)
        {
            rankingDatas.Add(RankingData.ranks[i]);
        }
    }

    public void UpdataUserData(Define.UpdateDateSort dateSort = Define.UpdateDateSort.RecodingGameResult, UserInfoData _data = null)
    {
        Debug.Log(_data.ToString());

        switch (dateSort)
        {
            case Define.UpdateDateSort.UsingDia:
                StartCoroutine(CoUsingDia(_data.freeDia, _data.payDia));
                break;
            case Define.UpdateDateSort.UsingHeart:
                StartCoroutine(CoUsingHeart(_data.heart));
                break;
            case Define.UpdateDateSort.ChangeNick:
                StartCoroutine(CoChangeNick(_data));
                break;
            case Define.UpdateDateSort.RecodingGameResult:
                StartCoroutine(CoRecodingGameResult(_data));
                break;
        }
    }

    IEnumerator CoUsingDia(int afterFreeDia, int afterPayDia)
    {
        yield break;
    }

    IEnumerator CoUsingHeart(int afterHeart)
    {
        yield break;
    }

    IEnumerator CoChangeNick(UserInfoData afterUserData)
    {
        yield break;
    }

    IEnumerator CoRecodingGameResult(UserInfoData afterGame)
    {
        yield break;
    }

    // Ż��
    public void DeleteUserData()
    {
        // ��ŷ���� ���� ����
    }

}
