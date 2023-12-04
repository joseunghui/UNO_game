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

        return result.ToString();
    }
}
#endregion
public class UserDataIns
{
    private static UserDataIns _instance = null;
    public static UserInfoData userInfo;
    private string userInfoDataRowInData = string.Empty;
    #region UserInfo Data Instance
    public static UserDataIns Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UserDataIns();
            }
            return _instance;
        }
    }
    #endregion

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
        RankingData.Instance.InsertRanking(0);
    }
    #endregion
    #region Get user all data
    public void GetMyAllData()
    {
        var bro = Backend.GameData.GetMyData("user", new Where()); // game data
        var getNick = Backend.BMember.GetUserInfo();

        // 실패 처리
        if (bro.IsSuccess() == false || getNick.IsSuccess() == false)
            Debug.LogError(bro);

        // 조회는 성공 했는데 데이터가 없는 경우
        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
            return;

        userInfo = new UserInfoData(); // 객체 초기화

        userInfo.nickname = getNick.GetReturnValuetoJSON()["row"]["nickname"].ToString();
        userInfo.nickChange = bool.Parse(bro.Rows()[0]["nickChange"]["BOOL"].ToString());
        userInfo.grade = int.Parse(bro.Rows()[0]["grade"]["N"].ToString());
        userInfo.heart = int.Parse(bro.Rows()[0]["heart"]["N"].ToString());
        userInfo.freeDia = int.Parse(bro.Rows()[0]["freeDia"]["N"].ToString());
        userInfo.payDia = int.Parse(bro.Rows()[0]["payDia"]["N"].ToString());
        userInfo.winrate = int.Parse(bro.Rows()[0]["winrate"]["N"].ToString());
        userInfo.totalCnt = int.Parse(bro.Rows()[0]["totalCnt"]["N"].ToString());
        userInfo.winCnt = int.Parse(bro.Rows()[0]["winCnt"]["N"].ToString());

        // 랭킹 업데이트

    }
    #endregion
    #region inDate로 유저 닉네임 조회
    public string GetUserNicknameToInDate(string _inDate)
    {
        var bro = Backend.Social.GetUserInfoByInDate(_inDate);
        return bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
    }
    #endregion
    #region user winrate change
    public bool updateUserNickname(string _nick, bool IsFree)
    {
        bool result = false;
        // 우선 중복 확인
        BackendReturnObject bro = Backend.BMember.CheckNicknameDuplication(_nick);

        if (bro.IsSuccess())
        {
            if (IsFree)
                UserNickChangeUpdate();

            var callback = Backend.BMember.UpdateNickname(_nick);

            if (callback.IsSuccess() == true)
                result = true;
        }
        return result;
    }
    #endregion
    #region user winrate change(change ranking)
    public void updateUserWinrate(int _totalCnt, int _winCnt)
    {
        int _winrate = (_winCnt / _totalCnt) * 100;
        Param param = new Param();
        param.Add("winrate", _winrate);
        param.Add("totalCnt", _totalCnt);
        param.Add("winCnt", _winCnt);
        
        Backend.GameData.Update("user", new Where(), param);
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
    public void UserDiaDataUpdate(UserInfoData changeDia)
    {
        Param param = new Param();

        param.Add("freeDia", changeDia.freeDia);
        param.Add("payDia", changeDia.payDia);

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

}
