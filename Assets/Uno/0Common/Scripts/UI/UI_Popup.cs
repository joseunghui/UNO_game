using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void init()
    {
        // �˾� �ȿ� �ű� ĵ���� ������Ʈ �����ϴ� ��� @UI_Root ����ϱ�
        // -> �� ��� ĵ���� ������Ʈ�� ���� ������Ʈ�� �־�� �ϴµ� @UI_Root�� �ذ� ���� 
        // Managers.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopup()
    {
        Managers.UI.ClosePopup(this);
    }
}
