using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
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
    public Button cardbtn;
    public Button turnbtn;
    enum ETurnMode {Random, My, Other}
    WaitForSeconds delay01 = new WaitForSeconds(0.1f);
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    public static Action<bool> OnAddCard;
    public static Action<bool> onStartCard;
    public static event Action<bool> OnTurnStarted;
    public AudioSource startSound;

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
        ColorBlock btnColor = turnbtn.colors;
        btnColor.normalColor = new Color32(55,55,55,255);

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
        StartCoroutine(StartTurnCo());
    }

    IEnumerator StartTurnCo(){
        isLoading = true;
        cardbtn.interactable = myTurn;
        ColorBlock btnColor = cardbtn.colors;
        btnColor.normalColor = myTurn ? new Color32(255,234,0,172) : new Color32(55,55,55,255);
        if(myTurn)
            GameManager.Inst.Notification("내 차례!");
        yield return delay05;
        isLoading = false;
        OnTurnStarted?.Invoke(myTurn);
    }

    public void EndTurn(){
        myTurn = !myTurn;
        StartCoroutine(StartTurnCo());
        turnbtn.interactable = false;
        ColorBlock btnColor1 = turnbtn.colors;
        btnColor1.normalColor = new Color32(55,55,55,255);
    }
}
