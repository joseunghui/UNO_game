using System;
using System.Text;
using UnityEngine;

using BackEnd; // 뒤끝 디비

#region UserInfoData Class
public class UserInfoData
{
    public string nickname;
    public int grade;
    public int heart;
    public int freeDia;
    public int payDia;
    public int totalCnt;
    public int winCnt;
    public float winrate;

    // data 디버깅 하기 위한 함수 (Debug.Log(UserData);)
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();

        result.AppendLine($"grade : {grade}");
        result.AppendLine($"heart : {heart}");
        result.AppendLine($"freeDia : {freeDia}");
        result.AppendLine($"payDia : {payDia}");
        result.AppendLine($"totalCnt : {totalCnt}");
        result.AppendLine($"winCnt : {winCnt}");
        result.AppendLine($"winrate : {winrate}");


        return result.ToString();
    }
}
#endregion
public class UserDataIns
{
    private static UserDataIns _instance = null;
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
        param.Add("grade", 0);
        param.Add("heart", 0);
        param.Add("freeDia", 10);
        param.Add("payDia", 0);
        param.Add("totalCnt", 0);
        param.Add("winCnt", 0);
        param.Add("winrate", 0.0);

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
    }
    #endregion
    #region Get user all data
    public UserInfoData GetMyAllData()
    {
        var bro = Backend.GameData.GetMyData("user", new Where()); // game data
        var getNick = Backend.BMember.GetUserInfo();

        // 실패 처리
        if (bro.IsSuccess() == false || getNick.IsSuccess() == false)
        {
            Debug.LogError(bro);
        }

        // 조회는 성공 했는데 데이터가 없는 경우
        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
        {
            // data 가 존재하는지 확인
            Debug.Log(bro);
        }

        // return Data
        UserInfoData myInfo = new UserInfoData();
        myInfo.nickname = getNick.GetReturnValuetoJSON()["row"]["nickname"].ToString();
        myInfo.grade = int.Parse(bro.Rows()[0]["grade"]["N"].ToString());
        myInfo.heart = int.Parse(bro.Rows()[0]["heart"]["N"].ToString());
        myInfo.freeDia = int.Parse(bro.Rows()[0]["freeDia"]["N"].ToString());
        myInfo.payDia = int.Parse(bro.Rows()[0]["payDia"]["N"].ToString());
        myInfo.totalCnt = int.Parse(bro.Rows()[0]["totalCnt"]["N"].ToString());
        myInfo.winCnt = int.Parse(bro.Rows()[0]["winCnt"]["N"].ToString());
        myInfo.winrate = float.Parse(bro.Rows()[0]["winrate"]["N"].ToString());

        for (int i=0; i<bro.Rows().Count; i++)
        {
            string inDate = bro.FlattenRows()[0]["inDate"].ToString();
            Debug.Log(inDate);
        }
        return myInfo;
    }
    #endregion
    #region inDate로 유저 닉네임 조회
    public string GetUserNicknameToInDate(string _inDate)
    {
        var bro = Backend.Social.GetUserInfoByInDate(_inDate);
        return bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
    }
    #endregion
    #region user nickname change
    public void updateUserNickname(string _nick)
    {
        // 우선 중복 확인
        BackendReturnObject bro = Backend.BMember.CheckNicknameDuplication(_nick);

        if (bro.IsSuccess())
        {
            Backend.BMember.UpdateNickname(_nick);
        }
    }
    #endregion
    #region userInfo update(change userInfo)
    public void UserDataUpdate(UserInfoData updateData)
    {
        Param param = new Param();

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
