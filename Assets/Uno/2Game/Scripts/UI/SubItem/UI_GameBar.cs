using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameBar : UI_SubItem
{
    string _nick;
    string _dia;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Define.Texts));
        Bind<Button>(typeof(Define.Buttons));

        GetText((int)Define.Texts.MyNicknameText).gameObject.GetComponent<TextMeshProUGUI>().text = _nick;
        GetText((int)Define.Texts.MyDiaText).gameObject.GetComponent<TextMeshProUGUI>().text = _dia;

        GetButton((int)Define.Buttons.OptionBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.Sound.Play("ButtonClick", Define.Sound.Effect);
            Managers.UI.ShowPopup<UI_Option>();
        });
    }

    public void SetUserData(string _tempNick, string _tempDia)
    {
        if (string.IsNullOrEmpty(_tempNick) && string.IsNullOrEmpty(_tempDia))
            return;

        _nick = _tempNick;
        _dia = _tempDia;
    }

    public void SetUserHeartDate(int _tempHeart)
    {
        if (_tempHeart < 0)
            return;

        Bind<GameObject>(typeof(Define.Groups));
        GameObject HeartIconList = Get<GameObject>((int)Define.Groups.HeartIconList).gameObject;

        for (int i=0; i<_tempHeart; i++)
        {
            Managers.Resource.Instantiate("UI/SubItem/Heart", parent: HeartIconList.transform);
        }
    }

}
