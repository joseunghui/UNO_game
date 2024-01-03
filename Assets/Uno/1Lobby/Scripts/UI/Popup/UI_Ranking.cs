using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;

public class UI_Ranking : UI_Popup
{
    private List<Ranking> rankings;
    private UserInfoData data;
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Load();
    }

    public void Load()
    {
        data = Managers.Data.GetUserInfoData();
        rankings = Managers.Data.GetAllRankingData();

        Bind<Button>(typeof(Define.Buttons));
        Bind<Image>(typeof(Define.Images));
        Bind<TextMeshProUGUI>(typeof(Define.Texts));
        Bind<GameObject>(typeof(Define.Groups));


        // mail button
        GetButton((int)Define.Buttons.MailBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Debug.Log("mail button click");
        });

        // shop button
        GetButton((int)Define.Buttons.ShopBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Debug.Log("shop button click");
        });


        // nick name change button
        GetButton((int)Define.Buttons.NickChangeBtn).gameObject.BindEvent((PointerEventData) =>
        {
            // nick name change popup open
            Managers.UI.ShowPopup<UI_NickChange>();
        });

        // pvc game start btn
        if (data.heart < 1)
            GetButton((int)Define.Buttons.EnterPVCGameBtn).interactable = false;
        // level select -> Move to Game Scene
        GetButton((int)Define.Buttons.EnterPVCGameBtn).gameObject.BindEvent((PointerEventData) =>
        {
            data.heart -= 1;
            Managers.Data.SetUserInfoData(data);
            Managers.Scene.LoadScene(Define.Scene.Game);
        });

        // pvp game start btn
        if (data.heart < 1)
            GetButton((int)Define.Buttons.EnterPVPGameBtn).interactable = false;
        // Move To Match Scene
        GetButton((int)Define.Buttons.EnterPVPGameBtn).gameObject.BindEvent((PointerEventData) =>
        {
            data.heart -= 1;
            Managers.Data.SetUserInfoData(data);
            Managers.Scene.LoadScene(Define.Scene.Match);
        });

        SetUserData();
        SetRankingData();
    }

    public void SetUserData()
    {
        GetText((int)Define.Texts.MyNicknameText).gameObject.GetComponent<TextMeshProUGUI>().text = data.nickname;

        // Goods Data (Heart, Dia)
        GameObject goodsList = Get<GameObject>((int)Define.Groups.GoodsList);

        foreach (Transform child in goodsList.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < 2; i++)
        {
            GameObject GoodsObject = Managers.UI.MakeSubItem<UI_Ranking_Goods>(parent: goodsList.transform).gameObject;

            GoodsObject.transform.SetParent(goodsList.transform);
            GoodsObject.transform.localScale = Vector3.one;

            UI_Ranking_Goods rankingGoodsItem = GoodsObject.GetOrAddComponent<UI_Ranking_Goods>();
            rankingGoodsItem._type = (Define.Goods)Enum.Parse(typeof(Define.Goods), Enum.GetName(typeof(Define.Goods), i));
        }
    }

    public void SetRankingData()
    {
        // ranking data 
        GameObject rankingList = Get<GameObject>((int)Define.Groups.RankingList);

        // 혹시 모르니 기존 데이터 날리기
        foreach (Transform child in rankingList.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < rankings.Count; i++)
        {
            GameObject RankingObject = Managers.UI.MakeSubItem<UI_Ranking_Ranker>(parent: rankingList.transform).gameObject;

            RankingObject.transform.SetParent(rankingList.transform);
            RankingObject.transform.localScale = Vector3.one;

            UI_Ranking_Ranker rankingObjectItem = RankingObject.GetOrAddComponent<UI_Ranking_Ranker>();
            rankingObjectItem.SetRankingData((i + 1).ToString(), rankings[i].user);
        }
    }



}
