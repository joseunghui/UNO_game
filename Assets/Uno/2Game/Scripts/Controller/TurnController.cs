using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnController : MonoBehaviour
{
    public int startCardCount;
    public bool isLoading; // ���� ������ true�� �ؼ� Ŭ�� ����
    public bool myTurn;
    public static Action<bool> OnAddCard;
    public static Action<bool> onStartCard;
    public static event Action<bool> OnTurnStarted;

    public int unoCount = 0;


    public void StartGame()
    {
        StartCoroutine(StartGameCo());
    }

    IEnumerator StartGameCo()
    {
        myTurn = Random.Range(0, 2) == 0;
        isLoading = true;

        /* �ּ� �ּ� �ѱ� �׽�Ʈ */
        Debug.Log($"myTurn >> {myTurn}");

        yield break;

        for (int i = 0; i < startCardCount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            OnAddCard?.Invoke(false);   // isMine; ���ī��
            yield return new WaitForSeconds(0.1f);
            OnAddCard?.Invoke(true);    // ��ī��
        }
        yield return new WaitForSeconds(0.1f);
        onStartCard?.Invoke(true);
        StartCoroutine(StartTurnCo());
    }

    IEnumerator StartTurnCo()
    {
        isLoading = true;

        // ButtonManager.Inst.turnStart(myTurn);

        yield return new WaitForSeconds(0.1f);
        isLoading = false;
        if (myTurn == false)
            yield return new WaitForSeconds(0.1f);
        OnTurnStarted?.Invoke(myTurn);
    }
}
