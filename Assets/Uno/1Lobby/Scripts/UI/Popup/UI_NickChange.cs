using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UI_NickChange : UI_Popup
{
    UserInfoData data;
    int neededDia;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        data = Managers.Data.GetUserInfoData();

        Bind<Image>(typeof(Define.Images));
        Bind<Button>(typeof(Define.Buttons));
        Bind<TMP_InputField>(typeof(Define.InputFields));
        Bind<TextMeshProUGUI>(typeof(Define.Texts));

        neededDia = int.Parse(GetText((int)Define.Texts.NeededDiaText).gameObject.GetComponent<TextMeshProUGUI>().text);
        Get<TMP_InputField>((int)Define.InputFields.NicknameInputField).placeholder.GetComponent<TextMeshProUGUI>().text = data.nickname;
        GetText((int)Define.Texts.MyDiaText).gameObject.GetComponent<TextMeshProUGUI>().text = (data.freeDia + data.payDia).ToString();
        
        GetButton((int)Define.Buttons.DoBtn).gameObject.BindEvent((PointerEventData) =>
        {
            if (data.freeDia + data.payDia < neededDia && data.nickChange == false)
                return;

            string updateNickname = GetText((int)Define.Texts.UpdateNicknameText).gameObject.GetComponent<TextMeshProUGUI>().text;
            ChangeNickExcu(updateNickname);
            
            GameObject rankingPage = GameObject.Find(Define.UI_Scene.UI_Ranking.ToString());

            Managers.UI.ClosePopup(rankingPage.GetOrAddComponent<UI_Ranking>());
            Managers.UI.ShowPopup<UI_Ranking>();
        });

        GetButton((int)Define.Buttons.CloseBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.UI.ClosePopup(this);
        });
    }


    #region ChangeNickExcu()
    private void ChangeNickExcu(string _nick)
    {
        Debug.Log($"1 before >> {data.nickname}");
        Debug.Log($"1 after >> {_nick}");

        if (string.IsNullOrEmpty(_nick))
            return;

        Managers.Data.SetUserNicknameInRanking(data.nickname, _nick);

        data.nickname = _nick;

        if (data.nickChange == false)
        {
            data.payDia = (data.freeDia - neededDia > 0) ? data.payDia : (data.freeDia - neededDia) + data.payDia;
            data.freeDia = (data.freeDia - neededDia > 0) ? data.freeDia - neededDia : 0;
        }

        Managers.Data.UpdataUserData(Define.UpdateDateSort.ChangeNick, data);
        Managers.UI.Clear();
    }
    #endregion
}
