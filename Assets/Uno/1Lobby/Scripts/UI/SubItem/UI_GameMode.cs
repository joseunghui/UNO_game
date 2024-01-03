using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GameMode : UI_SubItem
{
    string _mode;
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Define.Texts));

        GetText((int)Define.Texts.GameModeText).GetComponent<TextMeshProUGUI>().text = _mode;
    }

    public void SetGameModesetting(GameMode.PVCMode _type)
    {
        _mode = Enum.GetName(typeof(GameMode.PVCMode), _type);
    }
}
