using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class RankingDataManager : MonoBehaviour
{
    [Header("Before Game Popup")]
    [SerializeField] private GameObject BeforeGamePopup;
    [SerializeField] private Image GradeIcon;
    [SerializeField] private TextMeshProUGUI nicknameTxt;

    [SerializeField] private GameObject RankingList;
    [SerializeField] private GameObject RankerPrefab;

    [Header("Change Nickname Popup")]
    [SerializeField] private GameObject ChangeNickPopup;
    [SerializeField] private TextMeshProUGUI beforeNick;

    public Sprite Sliver;
    public Sprite Gold;

    List<RankingData> ranks;
    UserInfoData userInfoData;
    
    void Start()
    {
        StartCoroutine(SetTotalDataConnc());
    }

    #region ranking data
    IEnumerator SetTotalDataConnc()
    {
        yield return userInfoData = UserDataIns.Instance.GetMyAllData();
        yield return ranks = RankingDataIns.Instance.GetRankingData();

        nicknameTxt.text = userInfoData.nickname;
        beforeNick.text = userInfoData.nickname;

        if (userInfoData.grade == 1)
        {
            GradeIcon.GetComponent<Image>().sprite = Sliver;
        } else if (userInfoData.grade == 2)
        {
            GradeIcon.GetComponent<Image>().sprite = Gold;
        }


        for (int i=0; i<ranks.Count; i++)
        {
            GameObject rank = Instantiate(RankerPrefab, RankingList.transform.position, Quaternion.identity);

            rank.transform.localScale = new Vector3(0.012f, 0.012f, 0);
            rank.transform.SetParent(RankingList.transform);
        }

        List<RankingData> newRank = ranks.OrderBy(t => t.winrate).ToList();

        List<GameObject> rankLists = new List<GameObject>();
        for (int k=0; k< newRank.Count; k++)
        {
            rankLists.Add(RankingList.transform.GetChild(k).gameObject);
            int thisIndex = k;
            rankLists[k].transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = (thisIndex + 1).ToString();
            rankLists[k].transform.Find("RankerNick").GetComponent<TextMeshProUGUI>().text = newRank[thisIndex].ranker;
        }
    }
    #endregion
}
