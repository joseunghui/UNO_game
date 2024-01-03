using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GameBar_Timer : UI_SubItem
{
    string _timeer;
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Define.Texts));

        GetText((int)Define.Texts.TimerText).GetComponent<TextMeshProUGUI>().text = _timeer;
    }

    public void SetTimer(string timer)
    {
        _timeer = timer;
    }
}
