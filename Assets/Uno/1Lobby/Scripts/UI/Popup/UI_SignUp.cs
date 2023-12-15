using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SignUp : UI_Popup
{
    private string id_text;
    private string pw_text;

    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();

        // Resources > Prefabs > UI > Popup
        Bind<Button>(typeof(Define.Buttons));
        Bind<TextMeshProUGUI>(typeof(Define.Texts));
        Bind<TMP_InputField>(typeof(Define.InputFields));

        TextMeshProUGUI input_ID = Get<TextMeshProUGUI>((int)Define.Texts.IDText);
        TMP_InputField input_PW = Get<TMP_InputField>((int)Define.InputFields.PasswordInputField);

        GetButton((int)Define.Buttons.DoBtn).gameObject.BindEvent((PointerEventData) =>
        {
            id_text = input_ID.text;
            pw_text = input_PW.text;

            StartCoroutine(CoCustomSignUpProcess());
        });
    }

    IEnumerator CoCustomSignUpProcess()
    {
        if (pw_text != null && id_text != null)
        {
            Debug.Log($"id_text >> {id_text}");
            Debug.Log($"pw_text >> {pw_text}");

            if (Login.Instance.CustomSignUp(id_text, pw_text))
            {
                Managers.UI.ClosePopup();
                Managers.UI.ShowPopup<UI_SignIn>();
            }
        }
        yield break;
        
    }
}
