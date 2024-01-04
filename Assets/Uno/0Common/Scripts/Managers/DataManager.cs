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
        var bro = Backend.Initialize(true); // �ڳ� �ʱ�ȭ

        // �ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            Debug.Log("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 204 Success
        }
        else
        {
            Debug.LogError("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 400�� ���� �߻�
        }
    }

    private void Update()
    {
        Backend.AsyncPoll();
    }

    #region �α��� & ȸ������
    public bool CustomSignUp(string id, string pw)
    {
        Debug.Log("ȸ�������� ��û�մϴ�.");

        var bro = Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            // �г��� �ڵ� ����
            string randomNick = GetRandomDigit(6);
            Backend.BMember.CreateNickname(randomNick, (callback) =>
            {
                Debug.Log("ȸ�����Կ� �����߽��ϴ�. : " + bro);
            });
            return true;
        }
        else
        {
            Debug.LogError("ȸ�����Կ� �����߽��ϴ�. : " + bro);
            return false;
        }
    }

    // result �� �޾ƾ� ��
    public bool CustomLogin(string id, string pw)
    {
        Debug.Log("�α����� ��û�մϴ�.");

        var bro = Backend.BMember.CustomLogin(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("�α����� �����߽��ϴ�. : " + bro);
            return true;
        }
        else
        {
            Debug.LogError("�α����� �����߽��ϴ�. : " + bro);
            return false;
        }
    }


    // ���� ��� ���� �г��� ����
    public static string GetRandomDigit(int length)
    {
        string s = "user_";
        Random r = new Random((int)DateTime.Now.Ticks);

        //Random r = new Random(Convert.ToInt32(DateTime.Now.ToString("fffmmss")));
        int[] Random = new int[length];

        for (int i = 0; i < length; i++)
        {
            Random[i] = (int)r.Next(0, 10); //0���� ũ�ų� ����, 10���� ���� 
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

    // Ż��
    public void DeleteUserData()
    {
        // ��ŷ���� ���� ����
    }
    #endregion
}
