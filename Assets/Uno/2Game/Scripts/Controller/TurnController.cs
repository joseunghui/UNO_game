using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnController : MonoBehaviour
{
    [Header("Develop")]
    [SerializeField][Tooltip("시작 턴 모드를 정합니다")] ETurnMode eTurnMode;
    [SerializeField][Tooltip("시작 카드 개수를 정합니다")] int startCardCount;

    [Header("Properties")]
    public bool isLoading; // 게임 끝나면 true로 해서 클릭 방지
    public bool myTurn;

    enum ETurnMode { Random, My, Other }

    WaitForSeconds delay01 = new WaitForSeconds(0.1f);
    WaitForSeconds delay02 = new WaitForSeconds(1f);

    public static Action<bool> OnAddCard;
    public static Action<bool> onStartCard;
    public static event Action<bool> OnTurnStarted;

    public int unoCount = 0;

    void GameSetup()
    {
        switch (eTurnMode)
        {
            case ETurnMode.Random:
                myTurn = Random.Range(0, 2) == 0;
                break;
            case ETurnMode.My:
                myTurn = true;
                break;
            case ETurnMode.Other:
                myTurn = false;
                break;
        }
    }

    public IEnumerator StartGameCo()
    {
        GameSetup(); // 랜덤 시작 순서 지정
        isLoading = true;

        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay01;
            OnAddCard?.Invoke(false);   // isMine; 상대카드
            yield return delay01;
            OnAddCard?.Invoke(true);    // 내카드
        }
        yield return delay01;
        onStartCard?.Invoke(true);
        StartCoroutine(StartTurnCo());
    }

    IEnumerator StartTurnCo()
    {
        isLoading = true;
        // ButtonManager.Inst.turnStart(myTurn);
        yield return delay01;
        isLoading = false;
        if (myTurn == false)
            yield return delay02;
        OnTurnStarted?.Invoke(myTurn);
    }

    public void EndTurn()
    {
        /*myTurn = !myTurn;
        if (Card.myCards.Count == 0)
            StartCoroutine(GameOver(true));
        if (Card.otherCards.Count == 0)
            StartCoroutine(GameOver(false));
        else
            StartCoroutine(StartTurnCo());*/
    }

    void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            OnAddCard?.Invoke(true);
        if (Input.GetKeyDown(KeyCode.Keypad2))
            OnAddCard?.Invoke(false);
        if (Input.GetKeyDown(KeyCode.Keypad3))
            EndTurn();
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCo());
    }

    public IEnumerator GameOver(bool isMyWin)
    {
        isLoading = true;

        // 타이머 종료
        // Destroy(DataManager.Instance);

        // ButtonManager.Inst.endingPopUp(isMyWin);
        yield break;
    }

}
