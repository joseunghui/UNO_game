using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Turn : UI_SubItem
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Define.Texts));

        Get<TextMeshProUGUI>((int)Define.Texts.AddCardText).GetComponent<TextMeshProUGUI>().text = "ī�� ��������";
        Get<TextMeshProUGUI>((int)Define.Texts.EndTurnText).GetComponent<TextMeshProUGUI>().text = "�� �ѱ��";
    }
}
