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
        Debug.Log("SetTotalDataConnc() start!");
        yield return userInfoData = UserDataIns.Instance.GetMyAllData();
        yield return ranks = RankingDataIns.Instance.GetRankingData();

        nicknameTxt.text = userInfoData.nickname;

        if (userInfoData.grade == 1)
        {
            GradeIcon.GetComponent<Image>().sprite = Sliver;
        } else if (userInfoData.grade == 2)
        {
            GradeIcon.GetComponent<Image>().sprite = Gold;
        }

        for (int i=0; i<ranks.Count; i++)
        {
            Debug.Log($"rank {ranks[i].winrate} = {ranks[i].ranker}");
        }
    }
    #endregion
}
