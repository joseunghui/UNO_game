using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void init();

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // enum -> object name
        string[] names = Enum.GetNames(type);

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Utill.FindChild(gameObject, names[i], true);
            else
                objects[i] = Utill.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Binding Failed >> {names[i]}");
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    // ���� ����ϴ� ������Ʈ(Text, Button, Image)�� value�� �ٷ� �����ϰ� �Լ� ����
    // GameObject
    protected GameObject GetGameObject(int idx) { return Get<GameObject>(idx); }
    // Text
    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
    // Button
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    // Image
    protected Image GetImage(int idx) { return Get<Image>(idx); }


    // �̺�Ʈ ���ε� �ϴ� ��
    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Utill.GetOrAddComponent<UI_EventHandler>(go);

        switch(type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }
}
