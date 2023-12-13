using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopup()
    {
        Managers.UI.ClosePopup(this);
    }
}
