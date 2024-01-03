using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SignIn : UI_Popup
{
    private string id_text;
    private string pw_text;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

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

            StartCoroutine(CoCustomSignInProcess());
        });
    }

    IEnumerator CoCustomSignInProcess()
    {
        if (pw_text != null && id_text != null)
        {
            if (Managers.Data.CustomLogin(id_text, pw_text))
            {
                Managers.UI.ClosePopup();

                // ·Î±×ÀÎ ÈÄ¿¡´Â ·©Å· ÆË¾÷ open
                Managers.UI.ShowPopup<UI_Ranking>();
            }
        }
        yield break;

    }
}
