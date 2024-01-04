using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Mail_EA : UI_SubItem
{
    public Sprite HeartImage;
    public Sprite DiaImage;

    UserInfoData data;

    Sprite _rewardGoodsImage;
    int _rewardCount;
    string _rewardContent;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        data = Managers.Data.GetUserInfoData();

        Bind<Image>(typeof(Define.Images));
        Bind<TextMeshProUGUI>(typeof(Define.Texts));
        Bind<Button>(typeof(Define.Buttons));

        GetImage((int)Define.Images.GoodsIconImage).GetComponent<Image>().sprite = _rewardGoodsImage;
        GetText((int)Define.Texts.MailContentText).GetComponent<TextMeshProUGUI>().text = _rewardContent;
        GetText((int)Define.Texts.GoodsText).GetComponent<TextMeshProUGUI>().text = _rewardCount.ToString();

        GetButton((int)Define.Buttons.DoBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Debug.Log("Receive Button Click");
        });
    }

    public void SetMailEAData(Define.Goods Goods, int rewardCount, string rewardContent)
    {
        switch(Goods)
        {
            case Define.Goods.Heart:
                _rewardGoodsImage = HeartImage;
                break;
            case Define.Goods.Dia:
                _rewardGoodsImage = DiaImage;
                break;
        }

        _rewardContent = rewardContent;
        _rewardCount = rewardCount;
    }
}
