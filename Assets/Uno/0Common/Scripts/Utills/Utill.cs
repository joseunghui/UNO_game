using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utill
{
    // 해당 게임 오브젝트의 컴포넌트를 뽑아오거나 생성하기
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();

        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }

    // 해당 오브젝트의 자식인 게임 오브젝트만 가져오기(컴포넌트 아님)
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);

        if (transform == null)
            return null;
        return transform.gameObject;
    }

    // 해당 오브젝트의 하위를 가져오기(제너럴 타입이라 아무거나 가능)
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : Object
    {
        if (go == null)
            return null;

        if (recursive == false) // recursive = false : 자식의 자식 오브젝트는 찾지 않음(재귀X)
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
        else // recursive = true : 자식의 자식 오브젝트 찾음(재귀)
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
