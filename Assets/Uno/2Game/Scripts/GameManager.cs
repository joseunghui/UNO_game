using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 치트, UI, 랭킹, 게임오버 등등
public class GameManager : Singleton<GameManager>
{
    public static UserInfoData userInfo;

    void Start()
    {
        Debug.Log("Game Manager Start!");
        TurnManager.instance.StartGame();
        //userInfo = UserDataIns.Instance.GetMyAllData();
        //nickname.text = userInfo.nickname;
        //diaValue.text = (userInfo.freeDia + userInfo.payDia).ToString();

    }
    void Update(){

    }
}
