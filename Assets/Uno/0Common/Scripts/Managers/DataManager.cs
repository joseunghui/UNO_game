using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using static BackEnd.SendQueue;

public class DataManager
{
    public bool isLogin { get; private set; }   // 로그인 여부
    private string tempNickName;                        // 설정할 닉네임 (id와 동일)
    public string myNickName { get; private set; } = string.Empty;  // 로그인한 계정의 닉네임
    public string myIndate { get; private set; } = string.Empty;    // 로그인한 계정의 inDate
    private Action<bool, string> loginSuccessFunc = null;

    private const string BackendError = "statusCode : {0}\nErrorCode : {1}\nMessage : {2}";

    public string appleToken = ""; // SignInWithApple.cs에서 토큰값을 받을 문자열


    UserInfoDB _user = new UserInfoDB();
    RankingData _rank = new RankingData();
    PostDataDB _post = new PostDataDB();

    public UserInfoDB User { get { return _user; } }
    public RankingData Rank { get { return _rank; } }
    public PostDataDB Post { get { return _post; } }

    UserInfoData userInfoData;
    List<Ranking> rankingDatas;
    List<PostData> postDatas;



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

    // 뒤끝 토큰으로 로그인
    public void BackendTokenLogin(Action<bool, string> func)
    {
        Enqueue(Backend.BMember.LoginWithTheBackendToken, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("토큰 로그인 성공");
                loginSuccessFunc = func;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.Log("토큰 로그인 실패\n" + callback.ToString());
            func(false, string.Empty);
        });
    }

    // 난수 사용 최초 닉네임 설정
    public static string GetRandomDigit(int length)
    {
        string s = "user_";
        Random r = new Random((int)DateTime.Now.Ticks);

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

    // 유저 정보 불러오기 사전작업
    public void OnPrevBackendAuthorized()
    {
        Debug.Log("유저 정보 불러오기 사전작업");
        isLogin = true;

        OnBackendAuthorized();
    }

    // 실제 유저 정보 불러오기
    private void OnBackendAuthorized()
    {
        Debug.Log("OnBackendAuthorized()");

        Backend.BMember.GetUserInfo( callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError("유저 정보 불러오기 실패\n" + callback);
                loginSuccessFunc(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
                return;
            }
            Debug.Log("유저정보\n" + callback);

            var info = callback.GetReturnValuetoJSON()["row"];
            if (info["nickname"] == null)
            {
                return;
            }
            myNickName = info["nickname"].ToString();
            myIndate = info["inDate"].ToString();

            if (loginSuccessFunc != null)
            {
                Debug.Log($"loginSuccessFunc >> {loginSuccessFunc}");
                Managers.Match.GetMatchList(loginSuccessFunc);
            }
        });
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
        for (int i = 0;i < rankingDatas.Count;i++)
        {
            if (rankingDatas[i].user.Equals(_before))
            {
                rankingDatas[i].user = _after;
            }
        }
    }

    public List<PostData> GetPostDataList()
    {
        Debug.Log($"GetPostDataList()");

        Post.PostListGet(PostType.Admin);

        postDatas = Post.postList;
        Debug.Log($"1 postDatas >> {postDatas.Count}");

        return postDatas;
    }


    public void UpdataUserData(Define.UpdateDateSort dateSort = Define.UpdateDateSort.RecodingGameResult, UserInfoData _data = null)
    {
        // Debug.Log(_data.ToString());

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
            case Define.UpdateDateSort.PostReward:
                ReceivePost(_data);
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

        if (afterHeart >= 5)
        {
            userInfoData.heart = 5;
            return;
        }
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

    void ReceivePost(UserInfoData afterUserData)
    {
        User.UserReceivePost(afterUserData.heart, afterUserData.freeDia);
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
