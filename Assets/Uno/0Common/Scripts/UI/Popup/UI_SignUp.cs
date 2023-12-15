using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SignUp : UI_Popup
{
    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();

        // Resources > Prefabs > UI 
        Bind<GameObject>(typeof(Define.SignUpPopup));

        Get<InputField>((int)Define.SignUpPopup.IDInputField).interactable = true;
        Get<InputField>((int)Define.SignUpPopup.PasswordInputField).interactable = true;

        GetButton((int)Define.SignUpPopup.DoBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Debug.Log("Click");
        });
        


        // 이벤트 연결 -> 가입 완료 후 로그인
    }
}
