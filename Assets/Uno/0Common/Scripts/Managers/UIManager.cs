using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

// UI sorting + Load �� ����ϴ� �Ŵ���
public class UIManager
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

    public T ShowSceneInOldCanvas<T>(Transform parent = null, string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        T sceneUI = Utill.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(parent.transform);

        return sceneUI;
    }

    // Open
    public T ShowPopupOld<T>(string name = null) where T : UI_Popup
        // ���� ������Ʈ�� ������Ʈ�� Add �� ��ũ��Ʈ�� �̸��� �����ϱ� ������
        // �׳� ������Ʈ�� �̸����� ��ũ��Ʈ�� �������Բ� name�� ���� �ѱ��� ������ null ó��
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}"); // Scene�� ������(GameObject) ����

        T popup = Utill.GetOrAddComponent<T>(go); // ���ʸ� Ÿ���� popup���� ������Ʈ(Component) ����

        _popupStack.Push(popup); // ť�� ���������� �ֱ�

        go.transform.SetParent(Root.transform);

        return null;
    }
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
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
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

    // SubItem ���� Instantiate() v3
    public T MakeSubItemInTop<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;


        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        // ������ �̹� �ִ� ĵ���� �ȿ� SubItem�� ����� ���
        GameObject parentCanvas = GameObject.FindWithTag("Top");

        go.transform.SetParent(parentCanvas.transform);

        return Utill.GetOrAddComponent<T>(go);
    }

    // SubItem ���� Instantiate() v4
    public T MakeSubItemInContent<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;


        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        // ������ �̹� �ִ� ĵ���� �ȿ� SubItem�� ����� ���
        GameObject parentCanvas = GameObject.FindWithTag("Content");

        go.transform.SetParent(parentCanvas.transform);

        return Utill.GetOrAddComponent<T>(go);
    }


    public T CardSpawn<T>(Transform parent, Vector3 _pos, Quaternion _quat, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        if (parent == null) // Ȥ�ó� ���� ��ġ �������� ���� �׳� Content�ȿ� ����
            parent = GameObject.FindWithTag("Content").transform;

        GameObject go = Managers.Resource.Instantiate($"UI/Card/{name}", parent);
        
        go.transform.localScale = Vector3.one;
        go.transform.localPosition= _pos;
        go.transform.localRotation = _quat;

        return go as T;
    }

    public T CreatePostionSpot<T>(string name = null, Define.CardPRS _prs = Define.CardPRS.Left) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Transform/{name}");
        GameObject parentCanvas = GameObject.FindWithTag("Canvas");

        switch (_prs)
        {
            case Define.CardPRS.Left:
                go.transform.localRotation = new Quaternion(0f, 0f, 15f, 0f);
                break;
            case Define.CardPRS.Right:
                go.transform.localRotation = new Quaternion(0f, 0f, -15f, 0f);
                break;
        }
        go.transform.SetParent(parentCanvas.transform);
        go.transform.localScale = parentCanvas.transform.localScale;
        
        return go as T;
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
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
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
