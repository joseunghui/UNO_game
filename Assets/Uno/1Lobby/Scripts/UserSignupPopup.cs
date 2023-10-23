using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro; 

public class UserSignupPopup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
      // transform 의 scale 값을 모두 0.1f로 변경합니다.
      transform.localScale = Vector3.one * 0.1f;
      // 객체를 비활성화 합니다.
      gameObject.SetActive(false);
    }

    public void Show() 
    {
        gameObject.SetActive(true);
        var seq = DOTween.Sequence();

        seq.Append(transform.DOScale(1.1f, 0.2f));
        seq.Append(transform.DOScale(1f, 0.1f));

        seq.Play();
    }

    public void Hide()
    {
        var seq = DOTween.Sequence();

        transform.localScale = Vector3.one * 0.2f;

        seq.Append(transform.DOScale(1.1f, 0.1f));
        seq.Append(transform.DOScale(0.2f, 0.2f));

        seq.Play().OnComplete(() => 
        {
            gameObject.SetActive(false);
        });
    }
}
