using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Ranking : UI_Popup
{
    private List<Ranking> rankings;
    private UserInfoData data;
    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();

        // data setting
        data = Managers.Data.userInfoData;
        rankings = Managers.Data.rankingDatas;

        Bind<Button>(typeof(Define.Buttons));
        Bind<Image>(typeof(Define.Images));
        Bind<TextMeshProUGUI>(typeof(Define.Texts));
        Bind<GameObject>(typeof(Define.Groups));

        GetText((int)Define.Texts.MyNicknameText).gameObject.GetComponent<TextMeshProUGUI>().text = data.nickname;
        
        // nick name change button
        GetButton((int)Define.Buttons.NickChangeBtn).gameObject.BindEvent((PointerEventData) =>
        {
            // nick name change popup open
            Managers.UI.ShowPopup<UI_NickChange>();
        });

        // pvc game start btn
        // level select -> Move to Game Scene
        GetButton((int)Define.Buttons.EnterPVCGameBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.Scene.LoadScene(Define.Scene.Game);
        });

        // pvp game start btn
        // Move To Match Scene
        GetButton((int)Define.Buttons.EnterPVPGameBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.Scene.LoadScene(Define.Scene.Match);
        });


        // ranking data 
        GameObject rankingList = Get<GameObject>((int)Define.Groups.RankingList);

        // 혹시 모르니 기존 데이터 날리기
        foreach (Transform child in rankingList.transform)
            Managers.Resource.Destroy(child.gameObject);

        Debug.Log($"rankingList >> {rankingList.gameObject.name}");

        for (int i = 0; i < rankings.Count; i++)
        {
            GameObject RankingObject = Managers.UI.MakeSubItem<UI_Ranking_Ranker>(parent: rankingList.transform).gameObject;

            UI_Ranking_Ranker rankingObjectItem = RankingObject.GetOrAddComponent<UI_Ranking_Ranker>();
            rankingObjectItem.SetRankingData((i + 1).ToString(), rankings[i].user);
        }
    }


}
