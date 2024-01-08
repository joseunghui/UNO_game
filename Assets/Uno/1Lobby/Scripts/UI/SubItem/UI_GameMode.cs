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

        // GameModeText
        GetText((int)Define.Texts.GameModeText).gameObject.GetComponent<TextMeshProUGUI>().text = _mode;
    }

    public void SetGameModeSetting(GameMode.PVCMode _type)
    {
        Debug.Log($"SetGameModeSetting >> {_type}");
        _mode = _type.ToString();
    }
}
