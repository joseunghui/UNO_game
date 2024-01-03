using UnityEngine;

public class UI_SubItem : UI_Base
{
    public override void Init()
    {
        // 이미 있는 캔버스 가져오기
        // UI 가 씬에 노출 되려면 캔버스 컴포넌트를 가진 오브젝트 안에 있어야 함


        /**
         * [사용 규칙]
         * 1. 기존 캔버스의 태그를 
         * 2. 해당 SubItem의 스크립트에서 UI_SubItem을 상속 받는다 (UI_Popup, UI_Scene은 SetCanvas() 해주기 때문에 안됨)
         */
    }
}
