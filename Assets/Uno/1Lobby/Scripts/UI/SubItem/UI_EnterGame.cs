using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Battlehub.Dispatcher;

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
            AutoLoginIntoGame();

            Managers.Sound.Play("ButtonClick", Define.Sound.Effect);
            Managers.Resource.Destroy(gameObject);
        });
    }

    void AutoLoginIntoGame()
    {
        // 로그인 했으면 Game, 아니면 Login
        // 뒤끝 토큰으로 로그인
        Managers.Data.BackendTokenLogin((bool result, string error) =>
        {

            Dispatcher.Current.BeginInvoke(() =>
            {
                if (result)
                {
                    Managers.Data.Load();

                    // 로그인 후에는 랭킹 팝업 open
                    Managers.UI.ShowPopup<UI_Ranking>();

                    return;
                }

                if (!error.Equals(string.Empty))
                {
                    Debug.Log("유저 정보 불러오기 실패\n\n" + error);

                    Managers.Scene.LoadScene(Define.Scene.Login);
                    return;
                }

            });
        });
    }
}