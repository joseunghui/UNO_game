using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;

using BackEnd; // 뒤끝 디비

#region UserInfoData Class
public class UserInfoData
{
    public string email;
    public int winrate;
    public int grade;
    public int heart;
    public int freeDia;
    public int payDia;

    // data 디버깅 하기 위한 함수 (Debug.Log(UserData);)
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
       
        result.AppendLine($"email : {email}");
        result.AppendLine($"winrate : {winrate}");
        result.AppendLine($"grade : {grade}");
        result.AppendLine($"heart : {heart}");
        result.AppendLine($"freeDia : {freeDia}");
        result.AppendLine($"payDia : {payDia}");

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
    public void InsertUserData(UserInfoData userInfoData)
    {
        if (userInfoData == null)
        {
            userInfoData = new UserInfoData();
        }

        // 업데이트 목록에 추가하기 위한 param 생성
        Param param = new Param();
        param.Add("email", userInfoData.email); 
        param.Add("winrate", userInfoData.winrate);
        param.Add("grade", userInfoData.grade);
        param.Add("heart", userInfoData.heart);
        param.Add("freeDia", userInfoData.freeDia);
        param.Add("payDia", userInfoData.payDia);

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
    #region HavingThisUser where email
    public bool HavingThisUser(string _email)
    {
        Where where = new Where();
        where.Equal("email", _email);

        var bro = Backend.GameData.GetMyData("user", where, 1);

        if (bro.IsSuccess() == false)
        {
            Debug.Log(bro);
            return false;
        }

        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
        {
            // 기존 회원이 아님
            Debug.Log("회원가입을 해주세요.");
        } else {
            return true;
        }
        return false;
    }
    #endregion
    #region userInfo select() where nick
    public UserInfoData UserDataGet(string _nick)
    {
        var bro = Backend.Social.GetUserInfoByNickName(_nick);

        string gamerIndate = bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();

        UserInfoData info = new UserInfoData();
        if (bro.GetReturnValuetoJSON()["row"]["email"] != null)
        {
            string winrateStr = bro.GetReturnValuetoJSON()["row"]["winrate"].ToString();
            string gradeStr = bro.GetReturnValuetoJSON()["row"]["grade"].ToString();
            string heartStr = bro.GetReturnValuetoJSON()["row"]["heart"].ToString();
            string freeDiaStr = bro.GetReturnValuetoJSON()["row"]["freeDia"].ToString();
            string payDiaStr = bro.GetReturnValuetoJSON()["row"]["payDia"].ToString();

            info.email = bro.GetReturnValuetoJSON()["row"]["email"].ToString();
            info.winrate = Convert.ToInt32(winrateStr);
            info.grade = Convert.ToInt32(gradeStr);
            info.heart = Convert.ToInt32(heartStr);
            info.freeDia = Convert.ToInt32(freeDiaStr);
            info.payDia = Convert.ToInt32(payDiaStr);
        }
        return info;
    }
    #endregion
    #region userInfo update(change userInfo)
    public void UserDataUpdate()
    {

    }
    #endregion
}
