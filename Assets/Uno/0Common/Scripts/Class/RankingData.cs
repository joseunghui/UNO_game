using System;
using UnityEngine;
using System.Collections.Generic;
using BackEnd;
using System.Text; // 뒤끝 디비

public class Ranking
{
    public int rank;
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
    string rowInDate = string.Empty;
    public static List<Ranking> ranks = new List<Ranking>();
    #endregion

    #region Init Insert Ranking Data
    public void InsertRanking(int tempWinrate)
    {
        // 랭킹을 삽입하기 위해서는 게임 데이터에서 사용하는 데이터의 inDate값이 필요합니다.  
        // 따라서 데이터를 불러온 후, 해당 데이터의 inDate값을 추출하는 작업을 해야합니다.  
        Debug.Log("데이터 조회를 시도합니다.");
        var bro = Backend.GameData.GetMyData(tableName, new Where());

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("데이터 조회 중 문제가 발생했습니다 : " + bro);
            return;
        }

        Debug.Log("데이터 조회에 성공했습니다 : " + bro);

        if (bro.FlattenRows().Count > 0)
        {
            rowInDate = bro.FlattenRows()[0]["inDate"].ToString();
        }
        else
        {
            Debug.Log("데이터가 존재하지 않습니다. 데이터 삽입을 시도합니다.");
            var bro2 = Backend.GameData.Insert(tableName);

            if (bro2.IsSuccess() == false)
            {
                Debug.LogError("데이터 삽입 중 문제가 발생했습니다 : " + bro2);
                return;
            }

            Debug.Log("데이터 삽입에 성공했습니다 : " + bro2);

            rowInDate = bro2.GetInDate();
        }

        Debug.Log("내 게임 정보의 rowInDate : " + rowInDate); // 추출된 rowIndate의 값은 다음과 같습니다.  

        Param param = new Param();
        param.Add("winrate", tempWinrate);

        // 추출된 rowIndate를 가진 데이터에 param값으로 수정을 진행하고 랭킹에 데이터를 업데이트합니다.  
        Debug.Log("랭킹 삽입을 시도합니다.");
        var rankBro = Backend.URank.User.UpdateUserScore(rankUUID, tableName, rowInDate, param);

        if (rankBro.IsSuccess() == false)
        {
            Debug.LogError("랭킹 등록 중 오류가 발생했습니다. : " + rankBro);
            return;
        }

        Debug.Log("랭킹 삽입에 성공했습니다. : " + rankBro);
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
        Debug.Log("랭킹 조회에 성공했습니다. : " + bro);

        Debug.Log("총 랭킹 등록 유저 수 : " + bro.GetFlattenJSON()["totalCount"].ToString());

        foreach (LitJson.JsonData jsonData in bro.FlattenRows())
        {
            Ranking rank = new Ranking();
            rank.rank = int.Parse(jsonData["rank"].ToString());
            rank.user = jsonData["nickname"].ToString();
            rank.score = int.Parse(jsonData["score"].ToString());

            ranks.Add(rank);

            StringBuilder info = new StringBuilder();
            info.AppendLine("순위 : " + jsonData["rank"].ToString());
            info.AppendLine("닉네임 : " + jsonData["nickname"].ToString());
            info.AppendLine("점수 : " + jsonData["score"].ToString());
            info.AppendLine("gamerInDate : " + jsonData["gamerInDate"].ToString());
            info.AppendLine("정렬번호 : " + jsonData["index"].ToString());
            info.AppendLine();
            Debug.Log(info);
        }
    }
    #endregion
}
