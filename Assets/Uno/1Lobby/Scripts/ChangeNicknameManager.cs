using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeNicknameManager : MonoBehaviour
{
    [Header("Change Nickname Popup")]
    [SerializeField] private TextMeshProUGUI beforeNick;
    [SerializeField] private TextMeshProUGUI afterNick;

    private void Start()
    {
        beforeNick.text = RankingDataManager.Instance.userInfoData.nickname;
    }

    public void ChangeNickConfirmBtnClick()
    {
        StartCoroutine(ChangeNickExcu());
    }

    IEnumerator ChangeNickExcu()
    {
        yield return null;
        if (afterNick.text == null || afterNick.text == "")
            yield break;

        UserDataIns.Instance.updateUserNickname(afterNick.text);
    }
}
