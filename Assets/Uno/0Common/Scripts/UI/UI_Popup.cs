using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void init()
    {
        // 팝업 안에 신규 캔버스 컴포넌트 생성하는 대신 @UI_Root 사용하기
        // -> 이 경우 캔버스 컴포넌트를 가진 오브젝트가 있어야 하는데 @UI_Root로 해결 가능 
        // Managers.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopup()
    {
        Managers.UI.ClosePopup(this);
    }
}
