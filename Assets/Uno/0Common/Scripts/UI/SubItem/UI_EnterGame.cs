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
        Bind<GameObject>(typeof(Define.EnterGame));
        Bind<Button>(typeof(Define.Buttons));
        Bind<Image>(typeof(Define.Images));
        Bind<TextMeshProUGUI>(typeof(Define.Texts));

        GetText((int)Define.Texts.EnterGameText).GetComponent<TextMeshProUGUI>().text = "���ӽ���";
        GetButton((int)Define.Buttons.EnterGameButton).gameObject.BindEvent( (PointerEventData) =>
        {
            Debug.Log("Enter Game!");
            Managers.Scene.LoadScene(Define.Scene.Login);
        });

    }

}
