using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Ranking_Ranker : UI_SubItem
{
    string _rank;
    string _ranker;

    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();

        Bind<TextMeshProUGUI>(typeof(Define.Texts));

        GetText((int)Define.Texts.RankText).gameObject.GetComponent<TextMeshProUGUI>().text = _rank;
        GetText((int)Define.Texts.RankerNickText).gameObject.GetComponent<TextMeshProUGUI>().text = _ranker;
    }

    public void SetRankingData(string rank, string ranker)
    {
        _rank = rank;
        _ranker = ranker;
    }
}
