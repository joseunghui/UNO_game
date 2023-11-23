using System;
using UnityEngine;
using System.Collections.Generic;
using BackEnd; // 뒤끝 디비

public class RankingData
{
    public float winrate;
    public string ranker;
}

public class RankingDataIns
{
    private static RankingDataIns _instance = null;
    public static RankingDataIns Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RankingDataIns();
            }
            return _instance;
        }
    }

    public List<RankingData> GetRankingData()
    {
        List<RankingData> result = new List<RankingData>();
        string[] selectValues = { "totalCnt", "winCnt" };

        var GetUsersWinrateList = Backend.GameData.Get("user", new Where(), 100);

        if (GetUsersWinrateList.IsSuccess() == false)
        {
            Debug.Log(GetUsersWinrateList);
            return null;
        }

        if (GetUsersWinrateList.GetReturnValuetoJSON()["rows"].Count <= 0)
        {
            Debug.Log("nobody here");
            return null;
        }

        int GetWinCnt;
        int GetTotalCnt;

        for (int i = 0; i < GetUsersWinrateList.GetReturnValuetoJSON()["rows"].Count; i++)
        {
            RankingData data = new RankingData();

            GetWinCnt = int.Parse(GetUsersWinrateList.Rows()[i]["winCnt"]["N"].ToString()); // (int)GetUsersWinrateList.Rows()[i]["winCnt"]["N"];
            GetTotalCnt = int.Parse(GetUsersWinrateList.Rows()[i]["winCnt"]["N"].ToString()); // (int)GetUsersWinrateList.Rows()[i]["totalCnt"]["N"];

            data.winrate = (float)GetWinCnt / (float)GetTotalCnt;
            data.ranker = UserDataIns.Instance.GetUserNicknameToInDate(GetUsersWinrateList.Rows()[i]["owner_inDate"]["S"].ToString());

            result.Add(data);
        }

        return result;
    }
    

}
