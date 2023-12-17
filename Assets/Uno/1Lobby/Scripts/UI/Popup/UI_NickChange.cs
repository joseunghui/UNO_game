using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UI_NickChange : UI_Popup
{
    private UserInfoData data;
    int neededDia;
    int totalDia;

    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();

        data = Managers.Data.userInfoData;

        Debug.Log($"freeDia >> {data.freeDia}");
        Debug.Log($"payDia >> {data.payDia}");

        Bind<Image>(typeof(Define.Images));
        Bind<Button>(typeof(Define.Buttons));
        Bind<TMP_InputField>(typeof(Define.InputFields));
        Bind<TextMeshProUGUI>(typeof(Define.Texts));

        neededDia = 0;
        totalDia = data.freeDia + data.payDia;

        Get<TMP_InputField>((int)Define.InputFields.NicknameInputField).placeholder.GetComponent<TextMeshProUGUI>().text = data.nickname;
        
        GetText((int)Define.Texts.MyDiaText).gameObject.GetComponent<TextMeshProUGUI>().text = totalDia.ToString();
        neededDia = Int32.Parse(GetText((int)Define.Texts.NeededDiaText).gameObject.GetComponent<TextMeshProUGUI>().text);

        Button doBtn = GetButton((int)Define.Buttons.DoBtn);
        if (neededDia > totalDia)
            doBtn.interactable = false;

        doBtn.gameObject.BindEvent((PointerEventData) =>
        {
            string updateNickname = GetText((int)Define.Texts.UpdateNicknameText).gameObject.GetComponent<TextMeshProUGUI>().text;
            StartCoroutine("ChangeNickExcu", updateNickname);
        });

        GetButton((int)Define.Buttons.CloseBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.UI.ClosePopup(this);
        });
    }


    #region ChangeNickExcu()
    private IEnumerator ChangeNickExcu(string _nick)
    {
        if (data == null)
            yield break;

        if (data.nickname == _nick)
            yield break;

        if (totalDia < neededDia)
            yield break;

        Managers.Data.UpdataUserData(Define.UpdateDateSort.ChangeNick, data);

    }
    #endregion
}
