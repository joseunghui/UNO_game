using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public static Action<bool> OnAddCard;
    public static Action<bool> onStartCard;
    public static event Action<bool> OnTurnStarted;

    
    public void EndTurn()
    {
        CardController Card = gameObject.GetOrAddComponent<CardController>();

        // 턴 넘기기 or 시간 초과 -> 카드 뒷면 더미에서 4장 이상인지 체크 -> 진행

        if (Card.myCards.Count == 0)
            StartCoroutine(GameOver(true));
        if (Card.otherCards.Count == 0)
            StartCoroutine(GameOver(false));
        //else
            //StartCoroutine(StartTurnCo());
    }


    public IEnumerator GameOver(bool isMyWin)
    {
        // 타이머 종료
        // Destroy(DataManager.Instance);

        // ButtonManager.Inst.endingPopUp(isMyWin);
        yield break;
    }

}
