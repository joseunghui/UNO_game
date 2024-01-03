using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DataManager : MonoBehaviour
{
    public UserInfoDB _user = new UserInfoDB();
    public RankingData _rank = new RankingData();

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


    public void UpdataUserData(Define.UpdateDateSort dateSort = Define.UpdateDateSort.RecodingGameResult, UserInfoData _data = null)
    {
        Debug.Log(_data.ToString());

        switch (dateSort)
        {
            case Define.UpdateDateSort.UsingDia:
                StartCoroutine(CoUsingDia(_data.freeDia, _data.payDia));
                break;
            case Define.UpdateDateSort.UsingHeart:
                StartCoroutine(CoUsingHeart(_data.heart));
                break;
            case Define.UpdateDateSort.ChangeNick:
                StartCoroutine(CoChangeNick(_data));
                break;
            case Define.UpdateDateSort.RecodingGameResult:
                StartCoroutine(CoRecodingGameResult(_data));
                break;
        }
    }

    IEnumerator CoUsingDia(int afterFreeDia, int afterPayDia)
    {
        yield break;
    }

    IEnumerator CoUsingHeart(int afterHeart)
    {
        yield break;
    }

    IEnumerator CoChangeNick(UserInfoData afterUserData)
    {
        yield break;
    }

    IEnumerator CoRecodingGameResult(UserInfoData afterGame)
    {
        yield break;
    }

    // Ż��
    public void DeleteUserData()
    {
        // ��ŷ���� ���� ����
    }
    #endregion
}
