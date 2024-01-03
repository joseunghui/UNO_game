using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ranking_Goods : UI_SubItem
{
    public Define.Goods _type = Define.Goods.Heart; // basic = heart
    private UserInfoData tempUserInfo;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        tempUserInfo = Managers.Data.GetUserInfoData();

        Bind<Image>(typeof(Define.Images));
        Bind<TextMeshProUGUI>(typeof(Define.Texts));

        switch(_type)
        {
            case Define.Goods.Heart:
                break;
            case Define.Goods.Dia:
                GetImage((int)Define.Images.GoodsIconImage).gameObject.GetComponent<Image>().sprite = null;
                GetText((int)Define.Texts.GoodsText).gameObject.GetComponent<TextMeshProUGUI>().text = null;
                break;
        }

    }
}
