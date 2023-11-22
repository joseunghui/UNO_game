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
    private static string rankingUUID = "c4207d60-88f6-11ee-acce-7fbb598f7ba2";

    public List<RankingData> GetRankingData()
    {
        Debug.Log("GetRankingData() start!");

        List<RankingData> result = new List<RankingData>();
        string[] selectValues = { "totalCnt", "winCnt" };

        // bedonggi why andaenuagoooo whywhywhywhwywhywhwyhwywhywhywhwy
        Backend.GameData.Get("user", new Where(), selectValues, 100, bro =>
        {
            Debug.Log("???");
            if (bro.IsSuccess() == false)
            {
                Debug.Log(bro);
                return;
            }

            if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
            {
                Debug.Log("nobody here");
                return;
            }

            for (int i=0; i < bro.GetReturnValuetoJSON()["rows"].Count; i++)
            {
                RankingData data = new RankingData();
                data.winrate = (int)bro.Rows()[i]["winCnt"]["N"] / (int)bro.Rows()[i]["totalCnt"]["N"];
                data.ranker = UserDataIns.Instance.GetUserNicknameToInDate(bro.Rows()[i]["owner_inDate"]["S"].ToString());

                result.Add(data);
            }
        });

        return result;
    }
    /*
    public List<RankingData> GetRankingData2()
    {
        Debug.Log("GetRankingData() start!");
        List<RankingData> rankingDatas = new List<RankingData>();
        string rankerGamerId;

        Backend.URank.User.GetRankList(rankingUUID, 100, callback =>
        {
            Debug.Log(callback);
            if (callback.IsSuccess() == false)
                return;

            LitJson.JsonData rankListJson = callback.GetFlattenJSON();

            for (int i=0; i < rankListJson["rows"].Count; i++)
            {
                rankerGamerId = rankListJson["rows"][i]["gamerInDate"].ToString();

                RankingData data = new RankingData();

                Backend.Social.GetUserInfoByInDate(rankerGamerId, (callback) =>
                {
                    string nickname = callback.GetReturnValuetoJSON()["rows"]["nickname"].ToString();

                    data.rank = i + 1;
                    data.ranker = nickname;
                });
                rankingDatas.Add(data);
            }

            for (int j=0; j<rankingDatas.Count; j++)
            {
                Debug.Log($"ranking {rankingDatas[j].rank} = {rankingDatas[j].ranker}");
            }
        });
        return rankingDatas;
    }
    */

}
