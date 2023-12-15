using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI sorting + Load 를 담당하는 매니저
public class UIManager : MonoBehaviour
{
    int _order = 10;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    // @UI_Root 오브젝트를 폴더처럼 써서 담아두기
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");

            if (root == null)
            {
                root = new GameObject(); 
                root.transform.name = "@UI_Root";
                root.transform.position = Vector3.zero;
            }

            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        // 기존 캔버스 있으면 새로 만들지 않기
        if (GameObject.FindWithTag("Canvas") != null)
            return;

        Canvas canvas = Utill.GetOrAddComponent<Canvas>(go);
        CanvasOptionSetting(canvas);

        CanvasScaler canvasScaler = Utill.GetOrAddComponent<CanvasScaler>(go);
        CanvasScalerOptionSetting(canvasScaler);

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    // Open
    public T ShowScene<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        T sceneUI = Utill.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return null;
    }

    // Open
    public T ShowPopup<T>(string name = null) where T : UI_Popup
        // 보통 오브젝트와 오브젝트에 Add 된 스크립트의 이름이 동일하기 때문에
        // 그냥 오브젝트의 이름으로 스크립트를 가져오게끔 name을 따로 넘기지 않으면 null 처리
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject rootObject = Root;  // @UI_Root 생성
        Canvas canvas = Utill.GetOrAddComponent<Canvas>(rootObject);
        CanvasScaler canvasScaler = Utill.GetOrAddComponent<CanvasScaler>(rootObject);
        Utill.GetOrAddComponent<GraphicRaycaster>(rootObject);

        CanvasOptionSetting(canvas);
        CanvasScalerOptionSetting(canvasScaler);

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}", rootObject.transform); // Scene에 프리팹(GameObject) 생성

        T popup = Utill.GetOrAddComponent<T>(go); // 제너릭 타입의 popup으로 컴포넌트(Component) 생성

        _popupStack.Push(popup); // 큐에 순차적으로 넣기

        go.transform.SetParent(rootObject.transform);

        return null;
    }

    // SubItem 전용 Instantiate()
    public T MakeSubItemInNewCanvas<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        // parent 있으면 여기서 아예 SetParent() 까지 해주기
        if (parent != null)
        {
            go.transform.SetParent(parent);
        }

        return Utill.GetOrAddComponent<T>(go);
    }

    // SubItem 전용 Instantiate() v2
    public T MakeSubItemInOldCanvas<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        
        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        // 기존에 이미 있는 캔버스 안에 SubItem을 만드는 경우
        GameObject parentCanvas = GameObject.FindWithTag("Canvas");
        go.transform.SetParent(parentCanvas.transform);

        return Utill.GetOrAddComponent<T>(go);
    }


    // 특정 팝업 삭제
    public void ClosePopup(UI_Popup popup)
    {
        // 열려 있는 팝업 없으면 return
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != null)
        {
            Debug.Log("Close Popup Failed!");
        }

        ClosePopup();
    }

    // Close
    public void ClosePopup()
    {
        // 열려 있는 팝업 없으면 return
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop(); // 가장 먼저 위에 있는 팝업 뽑아오기(stack)

        Managers.Resource.Destroy(popup.gameObject); // 씬에서 삭제
        popup = null; // 혹시 모르니 null로 변경까지 해주기

        _order--;
    }

    // Close All
    public void CloseAllPopup()
    {
        // 열려 있는 팝업 없으면 return
        if (_popupStack.Count == 0)
            return;

        while (_popupStack.Count > 0)
        {
            ClosePopup();
        }
    }

    public void Clear()
    {
        CloseAllPopup();
        _sceneUI = null;
    }

    public void CanvasOptionSetting(Canvas canvas)
    {
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
    }

    public void CanvasScalerOptionSetting(CanvasScaler scaler)
    {
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        scaler.referencePixelsPerUnit = 100;
    }
}
