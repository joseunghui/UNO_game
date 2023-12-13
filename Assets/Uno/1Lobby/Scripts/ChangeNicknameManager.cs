using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ChangeNicknameManager : MonoBehaviour
{
    [Header("Change Nickname Popup")]
    [SerializeField] private GameObject ChangeNickPopup;
    [SerializeField] private GameObject CheckDiaField;
    [SerializeField] private TextMeshProUGUI beforeNick;
    [SerializeField] private TextMeshProUGUI afterNick;
    [SerializeField] private TextMeshProUGUI havingDia;
    [SerializeField] private TextMeshProUGUI neededDia;
    [SerializeField] private Button confirmBtn;
    private UserInfoData myData = new UserInfoData();

    private void Awake()
    {
        StartCoroutine(SetUserInfoData());
    }

    #region Set Data
    private IEnumerator SetUserInfoData()
    {
        UserDataIns.Instance.GetMyAllData();
        myData.nickname = UserDataIns.userInfo.nickname;
        myData.nickChange = UserDataIns.userInfo.nickChange;
        myData.freeDia = UserDataIns.userInfo.freeDia;
        myData.payDia = UserDataIns.userInfo.payDia;

        if (myData == null)
            yield break;

        SetUserInfoUI();
    }
    private void SetUserInfoUI()
    {
        beforeNick.text = myData.nickname;

        int havingDiaValue = myData.freeDia + myData.payDia;
        havingDia.text = havingDiaValue.ToString();

        if (myData.nickChange == true)
        {
            CheckDiaField.SetActive(false);
        }
        else
        {
            if (havingDiaValue < int.Parse(neededDia.text))
            {
                confirmBtn.interactable = false;
            }
        }
    }
    #endregion

    // 닉네임 변경 버튼
    public void ChangeNickConfirmBtnClick()
    {
        StartCoroutine(ChangeNickExcu());
    }

    #region ChangeNickExcu()
    private IEnumerator ChangeNickExcu()
    {
        if (afterNick.text == null || afterNick.text == "")
            yield break;


        // 업데이트 실행
        if (UserDataIns.Instance.updateUserNickname(afterNick.text, myData.nickChange) == true)
        {
            if (myData.nickChange == false)
            {
                UserInfoData tempUserInfo = new UserInfoData();

                int neededDiaValue = int.Parse(neededDia.text);

                if (myData.freeDia + myData.payDia > neededDiaValue)
                {
                    if (myData.freeDia - neededDiaValue > 0)
                    {
                        tempUserInfo.freeDia = myData.freeDia - neededDiaValue;
                        tempUserInfo.payDia = myData.payDia;
                    }
                    else
                    {
                        tempUserInfo.freeDia = 0;
                        tempUserInfo.payDia = myData.payDia - (neededDiaValue - myData.freeDia);
                    }
                }
                else
                {
                    beforeNick.text = "다이아가 부족합니다.";
                }

                UserDataIns.Instance.UserDiaDataUpdate(tempUserInfo);
            }
        }
        else
        {
            beforeNick.text = "이미 존재하는 닉네임 입니다.";
        }
        ChangeNickPopup.SetActive(false);
        Managers.Scene.LoadScene(Define.Scene.Main);
    }
    #endregion
}
