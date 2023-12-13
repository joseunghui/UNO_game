using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SignIn : UI_Popup
{
    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();

        // Resources > Prefabs > UI 
        Bind<UI_SignIn>(typeof(Define.Popups));

        UI_Popup signInPopup = Get<UI_Popup>((int)Define.Popups.UI_SignIn);

    }
}
