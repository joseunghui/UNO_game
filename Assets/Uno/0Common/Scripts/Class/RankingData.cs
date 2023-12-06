using System;
using UnityEngine;
using System.Collections.Generic;
using BackEnd;
using System.Text;
using System.Linq; // 뒤끝 디비

public class Ranking
{
    public string user;
    public int score;
}

public class RankingData
{
    #region instance & parameter
    private static RankingData _instance = null;
    public static RankingData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RankingData();
            }
            return _instance;
        }
    }
    string rankUUID = "c4207d60-88f6-11ee-acce-7fbb598f7ba2";
    string tableName = "user";
    public static List<Ranking> ranks = new List<Ranking>();
    #endregion

    #region Init Insert Ranking Data
    public void InsertRanking()
    {
        // 랭킹을 삽입하기 위해서는 게임 데이터에서 사용하는 데이터의 inDate값이 필요합니다.  
        // 따라서 데이터를 불러온 후, 해당 데이터의 inDate값을 추출하는 작업을 해야합니다.  
        var bro = Backend.GameData.GetMyData(tableName, new Where());

        string rowInDate = string.Empty;
        int winrate = 0;

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("데이터 조회 중 문제가 발생했습니다 : " + bro);
            return;
        }

        if (bro.FlattenRows().Count > 0)
        {
            rowInDate = bro.FlattenRows()[0]["inDate"].ToString();
            winrate = int.Parse(bro.FlattenRows()[0]["winrate"].ToString());
        }
        else
        {
            var bro2 = Backend.GameData.Insert(tableName);

            if (bro2.IsSuccess() == false)
            {
                Debug.LogError("데이터 삽입 중 문제가 발생했습니다 : " + bro2);
                return;
            }
            rowInDate = bro2.GetInDate();
        }

        Param param = new Param();
        param.Add("winrate", winrate);

        // 추출된 rowIndate를 가진 데이터에 param값으로 수정을 진행하고 랭킹에 데이터를 업데이트합니다.  
        var rankBro = Backend.URank.User.UpdateUserScore(rankUUID, tableName, rowInDate, param);

        if (rankBro.IsSuccess() == false)
        {
            Debug.LogError("랭킹 등록 중 오류가 발생했습니다. : " + rankBro);
            return;
        }
    }
    #endregion
    #region Select All Ranging Data
    public void RankingGet()
    {
        var bro = Backend.URank.User.GetRankList(rankUUID);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("랭킹 조회중 오류가 발생했습니다. : " + bro);
            return;
        }

        ranks = new List<Ranking>(); // 새로고침

        foreach (LitJson.JsonData jsonData in bro.FlattenRows())
        {
            Ranking rank = new Ranking();
            rank.user = jsonData["nickname"].ToString();
            rank.score = int.Parse(jsonData["score"].ToString());

            ranks.Add(rank);
        }

        // score 내림차순 정렬
        List<Ranking> sortedRanks = ranks.OrderByDescending(rank => rank.score).ToList();
        ranks = sortedRanks;
    }
    #endregion
}
