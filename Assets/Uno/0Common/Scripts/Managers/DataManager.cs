using BackEnd;
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
        userInfoData.heart = UserDataIns.userInfo.heart;
        userInfoData.grade = UserDataIns.userInfo.grade;
        userInfoData.freeDia = UserDataIns.userInfo.freeDia;
        userInfoData.payDia = UserDataIns.userInfo.payDia;
        userInfoData.totalCnt = UserDataIns.userInfo.totalCnt;
        userInfoData.winCnt = UserDataIns.userInfo.winCnt;

        RankingData.Instance.RankingGet();
        Debug.Log($"RankingData.ranks.Count : {RankingData.ranks.Count}");
        for (int i=0; i < RankingData.ranks.Count; i++)
        {
            rankingDatas.Add(RankingData.ranks[i]);
        }
    }


    public void UpdataUserData()
    {

    }

    public void DeleteUserData()
    {
        // ��ŷ���� ���� ����
    }

}
