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
        Bind<InputField>(typeof(Define.SignUpPopup));
        Bind<Button>(typeof(Define.SignUpPopup));
        Bind<TextMeshProUGUI>(typeof(Define.SignUpPopup));

        GetButton((int)Define.SignUpPopup.DoBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Debug.Log("Click");
        });
        


        // �̺�Ʈ ���� -> ���� �Ϸ� �� �α���
    }
}
