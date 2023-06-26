using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class NotificationPanel : MonoBehaviour
{
    [SerializeField] TMP_Text notificTMP;
    public void Show(string message){
        notificTMP.text = message;
        Sequence sequence = DOTween.Sequence()
        .Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad))
        .AppendInterval(0.9f)
        .Append(transform.DOScale(Vector3.zero, 03f).SetEase(Ease.InOutQuad));  //ease. .. 모양
    }
    void Start() => ScaleZero();

    [ContextMenu("ScaleOne")]   // 우클릭 제어
    void ScaleOne() => transform.localScale = Vector3.one;
    [ContextMenu("ScaleZero")]
    void ScaleZero() => transform.localScale = Vector3.zero;

}
