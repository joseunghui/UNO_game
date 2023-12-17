using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Ranking : UI_Popup
{
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

        Bind<Button>(typeof(Define.Buttons));
        Bind<Image>(typeof(Define.Images));
        Bind<TextMeshProUGUI>(typeof(Define.Texts));

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
    }


}
