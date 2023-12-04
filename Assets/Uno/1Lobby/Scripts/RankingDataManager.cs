using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class RankingDataManager : MonoBehaviour
{
    [Header("Before Game Popup")]
    [SerializeField] private GameObject BeforeGamePopup;
    [SerializeField] private Image GradeIcon;
    [SerializeField] private TextMeshProUGUI nicknameTxt;

    [SerializeField] private GameObject RankingList;
    [SerializeField] private GameObject RankerPrefab;

    public Sprite Sliver;
    public Sprite Gold;

    private List<Ranking> rankList;
    private UserInfoData userInfoData;
    public static int UserHeartCount;
    
    private void Awake()
    {
        StartCoroutine(SetTotalDataConnc());
    }

    #region SetTotalDataConnc()
    private IEnumerator SetTotalDataConnc()
    {
        UserDataIns.Instance.GetMyAllData();
        userInfoData = new UserInfoData();
        userInfoData.nickname = UserDataIns.userInfo.nickname;
        userInfoData.nickChange = UserDataIns.userInfo.nickChange;
        userInfoData.heart = UserDataIns.userInfo.heart;
        userInfoData.grade = UserDataIns.userInfo.grade;
        userInfoData.freeDia = UserDataIns.userInfo.freeDia;
        userInfoData.payDia = UserDataIns.userInfo.payDia;
        userInfoData.totalCnt = UserDataIns.userInfo.totalCnt;
        userInfoData.winCnt = UserDataIns.userInfo.winCnt;
        UserHeartCount = userInfoData.heart; // 하트만 퍼블릭 선언(상속받은 StartGame 스크립트에서 사용)

        RankingData.Instance.RankingGet();
        rankList = new List<Ranking>();
        for (int i = 0; i<RankingData.ranks.Count; i++)
        {
            rankList.Add(RankingData.ranks[i]);
        }

        if (userInfoData == null)
            yield break;

        StartCoroutine(SetRankingPopupUI());
    }
    #endregion
    #region SetRankingPopupUI()
    private IEnumerator SetRankingPopupUI()
    {
        if (userInfoData == null)
            yield break;

        nicknameTxt.text = userInfoData.nickname;
        if (userInfoData.grade == 1)
            GradeIcon.GetComponent<Image>().sprite = Sliver;
        else if (userInfoData.grade == 2)
            GradeIcon.GetComponent<Image>().sprite = Gold;

        ResetRankingList();

        for (int i = 0; i < rankList.Count; i++)
        {
            GameObject rank = Instantiate(RankerPrefab, RankingList.transform.position, Quaternion.identity);

            rank.transform.SetParent(RankingList.transform);
            rank.transform.localScale = RankingList.transform.localScale;
            rank.transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            rank.transform.Find("RankerNick").GetComponent<TextMeshProUGUI>().text = rankList[i].user;
        }
    }
    #endregion
    #region ResetRankingList()
    private void ResetRankingList()
    {
        var childs = RankingList.transform.GetComponentsInChildren<Transform>();

        foreach (var child in childs)
        {
            if (child != RankingList.transform)
            {
                Destroy(child); 
            }
        }
    }
    #endregion
}
