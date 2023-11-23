using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeNicknameManager : MonoBehaviour
{
    [Header("Change Nickname Popup")]
    [SerializeField] private GameObject ChangeNickPopup;
    [SerializeField] private TextMeshProUGUI afterNick;

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

        ChangeNickPopup.SetActive(false);
        LoadingSceneManager.LoadScene("MainScenes");
    }
}
