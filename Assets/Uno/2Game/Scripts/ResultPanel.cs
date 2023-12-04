using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    public static ResultPanel Inst {get; private set;}
    void Awake() => Inst = this;
    public TMP_Text resultTMP;

    public void Show(bool isMyWin){
        resultTMP.text = isMyWin ? "승리!" : "패배..";
        resultTMP.color = isMyWin ? new Color32(0,153,255,255) : new Color32(171,35,25,255);
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuad);
    }

    public void Restart(){
        gameObject.SetActive(false);
        LoadingSceneManager.LoadScene("mainScenes");
    }

    void Start() => ScaleZero();

    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;
    [ContextMenu("ScaleZero")]
    void ScaleZero() => transform.localScale = Vector3.zero;
}
