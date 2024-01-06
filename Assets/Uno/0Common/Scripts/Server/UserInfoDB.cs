using System;
using System.Text;
using UnityEngine;

using BackEnd; // 뒤끝 디비

#region UserInfoData Class
public class UserInfoData
{
    public string nickname;
    public bool nickChange;
    public int grade;
    public int heart;
    public int freeDia;
    public int payDia;
    public int winrate;
    public int totalCnt;
    public int winCnt;
    public DateTime heartChargeDt;

    // data 디버깅 하기 위한 함수 (Debug.Log(UserData);)
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();

        result.AppendLine($"nickname : {nickname}");
        result.AppendLine($"nickChange : {nickChange}");
        result.AppendLine($"grade : {grade}");
        result.AppendLine($"heart : {heart}");
        result.AppendLine($"freeDia : {freeDia}");
        result.AppendLine($"payDia : {payDia}");
        result.AppendLine($"winrate : {winrate}");
        result.AppendLine($"totalCnt : {totalCnt}");
        result.AppendLine($"winCnt : {winCnt}");
        result.AppendLine($"heartChargeDt : {heartChargeDt}");

        return result.ToString();
    }
}
#endregion
public class UserInfoDB
{
    public UserInfoData userInfo;
    private string userInfoDataRowInData = string.Empty;

    #region userInfo Insert(sign up)
    public void InsertUserData()
    {
        Param param = new Param();
        param.Add("nickChange", true); // 변경한 적 없으면 true, 있으면 false
        param.Add("grade", 0);
        param.Add("heart", 5);
        param.Add("freeDia", 10);
        param.Add("payDia", 0);
        param.Add("winrate", 0);
        param.Add("totalCnt", 0);
        param.Add("winCnt", 0);
        param.AddNull("heartChargeDt");

        // Insert excute
        Debug.Log("유저 DB Insert 실행");
        var bro = Backend.GameData.Insert("user", param);

        // 성공 or 실패
        if (bro.IsSuccess())
        {
            Debug.Log("계정 정보 데이터 삽입 성공 >> " + bro);

            // 삽입한 계정 정보의 고유 값
            userInfoDataRowInData = bro.GetInDate();
            Debug.Log("userInfo의 indate : " + userInfoDataRowInData);
        } else {
            Debug.Log("계정 정보 데이터 삽입 실패 >> " + bro);
        }

        // ranking date Insert
        Managers.Data.Rank.InsertRanking();
    }
    #endregion
    #region Get user all data
    public void GetMyAllData()
    {
        var bro = Backend.GameData.GetMyData("user", new Where()); // game data
        var getNick = Backend.UserNickName;

        // 실패 처리
        if (bro.IsSuccess() == false)
            Debug.LogError(bro);

        // 조회는 성공 했는데 데이터가 없는 경우
        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
            return;

        userInfo = new UserInfoData(); // 객체 초기화

        userInfo.nickname = getNick;
        userInfo.nickChange = bool.Parse(bro.Rows()[0]["nickChange"]["BOOL"].ToString());
        userInfo.grade = int.Parse(bro.Rows()[0]["grade"]["N"].ToString());
        userInfo.heart = int.Parse(bro.Rows()[0]["heart"]["N"].ToString());
        userInfo.freeDia = int.Parse(bro.Rows()[0]["freeDia"]["N"].ToString());
        userInfo.payDia = int.Parse(bro.Rows()[0]["payDia"]["N"].ToString());
        userInfo.winrate = int.Parse(bro.Rows()[0]["winrate"]["N"].ToString());
        userInfo.totalCnt = int.Parse(bro.Rows()[0]["totalCnt"]["N"].ToString());
        userInfo.winCnt = int.Parse(bro.Rows()[0]["winCnt"]["N"].ToString());

        if (!string.IsNullOrEmpty(bro.Rows()[0]["heartChargeDt"]["S"].ToString()))
            userInfo.heartChargeDt = DateTime.Parse(bro.Rows()[0]["heartChargeDt"]["S"].ToString());

        // 랭킹 업데이트

    }
    #endregion
    #region inDate 조회
    public string GetUserInDate()
    {
        var bro = Backend.BMember.GetUserInfo();

        if (bro.IsSuccess() == false)
            return null;

        LitJson.JsonData userInfoJson = bro.GetReturnValuetoJSON()["row"];
        return userInfoJson["inDate"].ToString();
    }
    #endregion
    #region Game Recoding Insert (게임 대진 기록)
    public void InsertGameRecoding(string loserUUID)
    {
        Param param = new Param();
        param.Add("loser", loserUUID); 

        var bro = Backend.GameData.Insert("game_recode", param);

        // 성공 or 실패
        if (bro.IsSuccess())
        {
            Debug.Log("대진 기록 데이터 삽입 실패 >> " + bro);
        }
        else
        {
            Debug.Log("대진 기록 데이터 삽입 실패 >> " + bro);
        }
    }
    #endregion
    #region Game Recoding lastGameAt 조회(마지막 하트 소모 시간 기록 가져오는 용도)
    public DateTime GetUserLastGameTime()
    {        
        var bro = Backend.PlayerData.GetMyData("game_recode", 1);

        if (bro.IsSuccess() == false)
            return new DateTime();

        return DateTime.Parse(bro.Rows()[0]["inDate"]["S"].ToString());
    }
    #endregion
    #region user nickname change
    public void updateUserNickname(string _nick, bool IsFree)
    {
        // 우선 중복 확인
        BackendReturnObject bro = Backend.BMember.CheckNicknameDuplication(_nick);

        if (bro.IsSuccess())
        {
            if (IsFree)
                UserNickChangeUpdate();

            var callback = Backend.BMember.UpdateNickname(_nick);

            if (callback.IsSuccess() == true)
                Debug.Log("nickname change success");
        }
    }
    #endregion
    #region user winrate change(change ranking)
    public void updateUserWinrate(int _totalCnt, int _winCnt)
    {
        int _winrate = (_winCnt / _totalCnt) * 100;
        Param userParam = new Param();
        userParam.Add("winrate", _winrate);
        userParam.Add("totalCnt", _totalCnt);
        userParam.Add("winCnt", _winCnt);
        
        Backend.GameData.Update("user", new Where(), userParam);

        // 랭킹 정보도 업데이트
        Param rankingParam = new Param();
        rankingParam.Add("winrate", _winrate);
        Backend.URank.User.UpdateUserScore("c4207d60-88f6-11ee-acce-7fbb598f7ba2", "user", GetUserInDate(), rankingParam);
    }
    #endregion
    #region user changeNick info update(change nick)
    public void UserNickChangeUpdate()
    {
        Param param = new Param();

        param.Add("nickChange", false);

        var bro = Backend.GameData.Update("user", new Where(), param);
    }
    #endregion
    #region user dia info update(change user's dia)
    public void UserDiaDataUpdate(int _freeDia, int _payDia)
    {
        Param param = new Param();

        param.Add("freeDia", _freeDia);
        param.Add("payDia", _payDia);

        var bro = Backend.GameData.Update("user", new Where(), param);
    }
    #endregion
    #region userInfo update(change userInfo)
    public void UserDataUpdate(UserInfoData updateData)
    {
        Param param = new Param();

        param.Add("nickChange", updateData.nickChange);
        param.Add("grade", updateData.grade);
        param.Add("heart", updateData.heart);
        param.Add("freeDia", updateData.freeDia);
        param.Add("payDia", updateData.payDia);
        param.Add("totalCnt", updateData.totalCnt);
        param.Add("winCnt", updateData.winCnt);

        Backend.GameData.Update("user", new Where(), param);
    }
    #endregion
    #region user's heart update(change userInfo)
    public void UserHeartDataUpdate(int tempHeart)
    {
        Param param = new Param();

        param.Add("heart", tempHeart);
        param.Add("heartChargeDt", DateTime.Now);

        Backend.GameData.Update("user", new Where(), param);
    }
    #endregion
    #region Post Receive 
    public void UserReceivePost(int tempHeart, int tempDia)
    {
        Param param = new Param();

        param.Add("heart", tempHeart);
        param.Add("freeDia", tempDia);

        Backend.GameData.Update("user", new Where(), param);
    }
    #endregion


    #region user connect recodeing(auto heart charge)
    public void UserHeartChargeStartDateUpdate()
    {
        Param param = new Param();

        param.Add("heartChargeDt", DateTime.Now);

        Backend.GameData.Update("user", new Where(), param);
    }
    #endregion
}
