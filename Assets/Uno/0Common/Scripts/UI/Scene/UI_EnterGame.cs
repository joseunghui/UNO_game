using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EnterGame : UI_Scene
{
    private void Start()
    {
        init();
    }
    public override void init()
    {
        base.init();

        Bind<GameObject>(typeof(Define.EnterGame));
        Bind<Button>(typeof(Define.Buttons));
        Bind<Image>(typeof(Define.Images));
        Bind<TextMeshProUGUI>(typeof(Define.Texts));

        GetText((int)Define.Texts.EnterGameText).GetComponent<TextMeshProUGUI>().text = "게임시작";
        GetButton((int)Define.Buttons.EnterGameButton).gameObject.BindEvent( (PointerEventData) =>
        {
            // 로그인 했으면 Game, 아니면 Login
            // 기등록된 로컬 기기 자동 로그인
            BackendReturnObject autoLogin = Backend.BMember.LoginWithTheBackendToken();

            if (autoLogin.IsSuccess())
            {
                Managers.Scene.LoadScene(Define.Scene.Game);
            }
            else
            {
                Managers.Scene.LoadScene(Define.Scene.Login);
            }
        });

    }

}
