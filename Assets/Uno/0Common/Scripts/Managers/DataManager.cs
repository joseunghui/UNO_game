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
        // 랭킹까지 같이 삭제
    }

}
