using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TurnManager : Singleton<TurnManager>
{
    /* sample
    [Header("status bar")]
    [SerializeField] public TextMeshProUGUI text;

    void Start()
    {
        UserInfoData user = new UserInfoData();
        user = UserDataIns.Instance.GetMyAllData();
        text.text = user.grade;
    }
    */

    [Header("Develop")]
    [SerializeField] [Tooltip("시작 턴 모드를 정합니다")] ETurnMode eTurnMode;
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다")] int startCardCount;

    [Header("Properties")]
    public bool isLoading; // 게임 끝나면 true로 해서 클릭 방지
    public bool myTurn;
    public Button cardbtn;
    public Button turnbtn;
    public Button unobtn;
    enum ETurnMode {Random, My, Other}
    WaitForSeconds delay01 = new WaitForSeconds(0.1f);
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    public static Action<bool> OnAddCard;
    public static Action<bool> onStartCard;
    public static event Action<bool> OnTurnStarted;
    public AudioSource startSound;
    int unoCount = 0;

    void GameSetup(){
        switch(eTurnMode){
            case ETurnMode.Random:
                myTurn = Random.Range(0,2) == 0;
                break;
            case ETurnMode.My:
                myTurn = true;
                break;
            case ETurnMode.Other:
                myTurn = false;
                break;
        }
    }

    public IEnumerator StartGameCo(){
        turnbtn.interactable = false;
        unobtn.interactable = false;
        cardbtn.interactable = false;

        GameSetup();
        isLoading = true;

        startSound.Play();
        
        for(int i=0; i<startCardCount; i++){
            yield return delay01;
            OnAddCard?.Invoke(false);   // isMine; 상대카드
            yield return delay01;
            OnAddCard?.Invoke(true);    // 내카드
        }
        yield return delay01;
        onStartCard?.Invoke(true);
        unoCount = 1;
        StartCoroutine(StartTurnCo());
    }

    IEnumerator StartTurnCo(){
        isLoading = true;
        cardbtn.interactable = myTurn;
        if(myTurn)
            GameManager.Inst.Notification("내 차례!");
        yield return delay05;
        isLoading = false;
        
        OnTurnStarted?.Invoke(myTurn);
    }

    public void Update(){
        if(CardManager.Inst.myCards.Count >= 2)
            unoCount = 1;
        if(CardManager.Inst.myCards.Count == 1 && unoCount == 1)
            unobtn.interactable = true;
    }

    public void EndTurn(){
        myTurn = !myTurn;
        StartCoroutine(StartTurnCo());
        turnbtn.interactable = false;
    }

    public void Uno(){
        OnAddCard?.Invoke(false); // 게임이 내 기준에서 구현해서 일단 false로 넣어놓음
        OnAddCard?.Invoke(false);
        // 상대 움직임 구현에 따라 수정 필요
        unobtn.interactable = false;
        unoCount = 0;
    }
}
