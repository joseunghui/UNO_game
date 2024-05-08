using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MatchLoading : UI_Popup
{
    Button CancelMatchBtn;

    private void Start()
    {
        Init();

        Managers.Match.IsMatchGameActivate();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(Define.LoadgingVideo));
        Bind<Button>(typeof(Define.Buttons));

        CancelMatchBtn = Get<Button>((int)Define.Buttons.CloseBtn).GetComponent<Button>();

        CancelMatchBtn.onClick.AddListener(() => { ClickMatchCancelBtn(); });
    }

    void ClickMatchCancelBtn()
    {
        Managers.Match.LeaveInGameRoom();
        Managers.Match.LeaveMatchServer();

        Managers.UI.CloseAllPopup();
        Managers.UI.ShowPopup<UI_Ranking>();
    }
}
