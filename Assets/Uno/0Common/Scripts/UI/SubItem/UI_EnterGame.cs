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
        init();
    }
    public override void init()
    {
        base.init();

        // �����;� �� �� : ��ư, �ؽ�Ʈ
        Bind<Button>(typeof(Define.Buttons));
        Bind<TextMeshProUGUI>(typeof(Define.Texts));

        GetText((int)Define.Texts.EnterGameText).GetComponent<TextMeshProUGUI>().text = "���ӽ���";
        GetButton((int)Define.Buttons.EnterGameButton).gameObject.BindEvent( (PointerEventData) =>
        {
            // �α��� ������ Game, �ƴϸ� Login
            // ���ϵ� ���� ��� �ڵ� �α���
            BackendReturnObject autoLogin = Backend.BMember.LoginWithTheBackendToken();

            if (autoLogin.IsSuccess())
            {
                // �α��� �Ŀ��� ��ŷ �˾� open
                Managers.UI.ShowPopup<UI_SignIn>();
            }
            else
            {
                Managers.Scene.LoadScene(Define.Scene.Login);
            }
        });

    }

}
