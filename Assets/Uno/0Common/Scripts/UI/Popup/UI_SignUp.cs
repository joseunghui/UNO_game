using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Bind<UI_Popup>(typeof(Define.Popups));

        UI_Popup signUpPopup = Get<UI_Popup>((int)Define.Popups.UI_SignUp);
    }
}
