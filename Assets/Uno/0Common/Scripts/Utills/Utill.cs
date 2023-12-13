using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utill
{
    // �ش� ���� ������Ʈ�� ������Ʈ�� �̾ƿ��ų� �����ϱ�
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();

        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }

    // �ش� ������Ʈ�� �ڽ��� ���� ������Ʈ�� ��������(������Ʈ �ƴ�)
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);

        if (transform == null)
            return null;
        return transform.gameObject;
    }

    // �ش� ������Ʈ�� ������ ��������(���ʷ� Ÿ���̶� �ƹ��ų� ����)
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : Object
    {
        if (go == null)
            return null;

        if (recursive == false) // recursive = false : �ڽ��� �ڽ� ������Ʈ�� ã�� ����(���X)
        {
            for (int i = 0; i<go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);

                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else // recursive = true : �ڽ��� �ڽ� ������Ʈ ã��(���)
        {
            foreach(T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty (name) || component.name == name)
                {
                    return component;
                }
            }    
        }

        return null;
    }
}
