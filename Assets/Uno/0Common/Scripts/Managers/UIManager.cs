using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI sorting + Load �� ����ϴ� �Ŵ���
public class UIManager : MonoBehaviour
{
    int _order = 10;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    // @UI_Root ������Ʈ�� ����ó�� �Ἥ ��Ƶα�
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
        // ���� ĵ���� ������ ���� ������ �ʱ�
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
        // ���� ������Ʈ�� ������Ʈ�� Add �� ��ũ��Ʈ�� �̸��� �����ϱ� ������
        // �׳� ������Ʈ�� �̸����� ��ũ��Ʈ�� �������Բ� name�� ���� �ѱ��� ������ null ó��
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject rootObject = Root;  // @UI_Root ����
        Canvas canvas = Utill.GetOrAddComponent<Canvas>(rootObject);
        CanvasScaler canvasScaler = Utill.GetOrAddComponent<CanvasScaler>(rootObject);
        Utill.GetOrAddComponent<GraphicRaycaster>(rootObject);

        CanvasOptionSetting(canvas);
        CanvasScalerOptionSetting(canvasScaler);

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}", rootObject.transform); // Scene�� ������(GameObject) ����

        T popup = Utill.GetOrAddComponent<T>(go); // ���ʸ� Ÿ���� popup���� ������Ʈ(Component) ����

        _popupStack.Push(popup); // ť�� ���������� �ֱ�

        go.transform.SetParent(rootObject.transform);

        return null;
    }

    // SubItem ���� Instantiate()
    public T MakeSubItemInNewCanvas<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        // parent ������ ���⼭ �ƿ� SetParent() ���� ���ֱ�
        if (parent != null)
        {
            go.transform.SetParent(parent);
        }

        return Utill.GetOrAddComponent<T>(go);
    }

    // SubItem ���� Instantiate() v2
    public T MakeSubItemInOldCanvas<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        
        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        // ������ �̹� �ִ� ĵ���� �ȿ� SubItem�� ����� ���
        GameObject parentCanvas = GameObject.FindWithTag("Canvas");
        go.transform.SetParent(parentCanvas.transform);

        return Utill.GetOrAddComponent<T>(go);
    }


    // Ư�� �˾� ����
    public void ClosePopup(UI_Popup popup)
    {
        // ���� �ִ� �˾� ������ return
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
        // ���� �ִ� �˾� ������ return
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop(); // ���� ���� ���� �ִ� �˾� �̾ƿ���(stack)

        Managers.Resource.Destroy(popup.gameObject); // ������ ����
        popup = null; // Ȥ�� �𸣴� null�� ������� ���ֱ�

        _order--;
    }

    // Close All
    public void CloseAllPopup()
    {
        // ���� �ִ� �˾� ������ return
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
