using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Inst {get; private set;}
    void Awake() => Inst = this;

    [Header("Develop")]
    [SerializeField] [Tooltip("시작 턴 모드를 정합니다")] ETurnMode eTurnMode;
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다")] int startCardCount;

    [Header("Properties")]
    public bool isLoading; // 게임 끝나면 true로 해서 클릭 방지
    public bool myTurn;
    enum ETurnMode {Random, My, Other}
    WaitForSeconds delay01 = new WaitForSeconds(0.1f);
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);

    public static Action<bool> OnAddCard;

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
        GameSetup();
        isLoading = true;
        
        for(int i=0; i<startCardCount; i++){
            yield return delay01;
            OnAddCard?.Invoke(false);   // isMine; 상대카드
            yield return delay01;
            OnAddCard?.Invoke(true);    // 내카드
        }
        StartCoroutine(StartTurnCo());
    }

    IEnumerator StartTurnCo(){
        isLoading = true;
        if(myTurn)
            GameManager.Inst.Notification("내 차례!");

        yield return delay05;
        //OnAddCard?.Invoke(myTurn);  턴마다 카드 하나 먹는 코드
        yield return delay05;
        isLoading = false;
    }

    public void EndTurn(){
        myTurn = !myTurn;
        StartCoroutine(StartTurnCo());
    }
}
