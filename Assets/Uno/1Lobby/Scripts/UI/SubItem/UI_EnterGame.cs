using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnterGame : UI_SubItem
{
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();

        // 가져와야 할 것 : 버튼, 텍스트
        Bind<Button>(typeof(Define.Buttons));
        Bind<TextMeshProUGUI>(typeof(Define.Texts));

        GetText((int)Define.Texts.EnterGameText).GetComponent<TextMeshProUGUI>().text = "게임시작";
        GetButton((int)Define.Buttons.EnterGameButton).gameObject.BindEvent( (PointerEventData) =>
        {
            Managers.Sound.Play("ButtonClick", Define.Sound.Effect);

            // 로그인 했으면 Game, 아니면 Login
            // 기등록된 로컬 기기 자동 로그인
            BackendReturnObject autoLogin = Backend.BMember.LoginWithTheBackendToken();

            if (autoLogin.IsSuccess())
            {
                Managers.Data.Load();

                Managers.Resource.Destroy(gameObject);
                // 로그인 후에는 랭킹 팝업 open
                Managers.UI.ShowPopup<UI_Ranking>();
            }
            else
            {
                Managers.Scene.LoadScene(Define.Scene.Login);
            }
        });

    }

}