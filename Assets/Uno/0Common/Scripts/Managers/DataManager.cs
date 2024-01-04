using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DataManager : MonoBehaviour
{
    UserInfoDB _user = new UserInfoDB();
    RankingData _rank = new RankingData();

    public UserInfoDB User { get { return _user; } }
    public RankingData Rank { get { return _rank; } }

    private UserInfoData userInfoData;
    private List<Ranking> rankingDatas;

    public void Init()
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

    #region 로그인 & 회원가입
    public bool CustomSignUp(string id, string pw)
    {
        Debug.Log("회원가입을 요청합니다.");

        var bro = Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            // 닉네임 자동 생성
            string randomNick = GetRandomDigit(6);
            Backend.BMember.CreateNickname(randomNick, (callback) =>
            {
                Debug.Log("회원가입에 성공했습니다. : " + bro);
            });
            return true;
        }
        else
        {
            Debug.LogError("회원가입에 실패했습니다. : " + bro);
            return false;
        }
    }

    // result 를 받아야 함
    public bool CustomLogin(string id, string pw)
    {
        Debug.Log("로그인을 요청합니다.");

        var bro = Backend.BMember.CustomLogin(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("로그인이 성공했습니다. : " + bro);
            return true;
        }
        else
        {
            Debug.LogError("로그인이 실패했습니다. : " + bro);
            return false;
        }
    }


    // 난수 사용 최초 닉네임 설정
    public static string GetRandomDigit(int length)
    {
        string s = "user_";
        Random r = new Random((int)DateTime.Now.Ticks);

        //Random r = new Random(Convert.ToInt32(DateTime.Now.ToString("fffmmss")));
        int[] Random = new int[length];

        for (int i = 0; i < length; i++)
        {
            Random[i] = (int)r.Next(0, 10); //0보다 크거나 같고, 10보다 작은 
        }

        for (int i = 0; i < length; i++)
        {
            s += Random[i].ToString();
        }
        return s;
    }
    #endregion

    #region Load Data
    public void Load()
    {
        LoadUserInfoData();
        LoadAllRankingData();
    }
    void LoadUserInfoData()
    {
        User.GetMyAllData();
        userInfoData = User.userInfo;
    }
    
    void LoadAllRankingData()
    {
        Rank.RankingGet();
        rankingDatas = Rank.ranks;
    }
    #endregion
    #region CRUD
    public void InsertUserData()
    {
        // Game Data Insert
        User.InsertUserData();
        Rank.InsertRanking();
    }
    public UserInfoData GetUserInfoData()
    {
        return userInfoData;
    }

    public void SetUserInfoData(UserInfoData pUserInfoData)
    {
        userInfoData = pUserInfoData;
    }

    public List<Ranking> GetAllRankingData()
    {
        return rankingDatas;
    }

    public void SetUserNicknameInRanking(string _before, string _after)
    {
        Debug.Log($"2 before >> {_before}");
        Debug.Log($"2 after >> {_after}");

        for (int i = 0;i < rankingDatas.Count;i++)
        {
            if (rankingDatas[i].user.Equals(_before))
            {
                rankingDatas[i].user = _after;
            }
        }
    }


    public void UpdataUserData(Define.UpdateDateSort dateSort = Define.UpdateDateSort.RecodingGameResult, UserInfoData _data = null)
    {
        Debug.Log(_data.ToString());

        switch (dateSort)
        {
            case Define.UpdateDateSort.UsingDia:
                UsingDia(_data.freeDia, _data.payDia);
                break;
            case Define.UpdateDateSort.UsingHeart:
                UsingHeart(_data.heart);
                break;
            case Define.UpdateDateSort.ChangeNick:
                ChangeNick(_data);
                break;
            case Define.UpdateDateSort.RecodingGameResult:
                RecodingGameResult(_data);
                break;
        }
    }

    void UsingDia(int afterFreeDia, int afterPayDia)
    {
        if (afterFreeDia + afterPayDia < 0)
            return;

        User.UserDiaDataUpdate(afterFreeDia, afterPayDia);
    }

    void UsingHeart(int afterHeart)
    {
        if (afterHeart < 0)
            return;

        userInfoData.heart = afterHeart;
        User.UserHeartDataUpdate(afterHeart);
    }

    void ChangeNick(UserInfoData afterUserData)
    {
        if (string.IsNullOrEmpty(afterUserData.nickname))
            return;

        if (afterUserData.nickChange)
        {
            User.updateUserNickname(afterUserData.nickname, afterUserData.nickChange);
            userInfoData.nickChange = false;
        }
        else
        {
            userInfoData.freeDia = afterUserData.freeDia;
            userInfoData.payDia = afterUserData.payDia;
            User.updateUserNickname(afterUserData.nickname, afterUserData.nickChange);
            User.UserDiaDataUpdate(afterUserData.freeDia, afterUserData.payDia);
        }

        userInfoData.nickname = afterUserData.nickname;
    }

    void RecodingGameResult(UserInfoData afterGame)
    {
        return;
    }

    // 탈퇴
    public void DeleteUserData()
    {
        // 랭킹까지 같이 삭제
    }
    #endregion
}
