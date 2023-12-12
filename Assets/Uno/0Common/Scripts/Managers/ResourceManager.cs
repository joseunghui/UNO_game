using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        // 원본을 메모리에 로드(original)
        GameObject original = Load<GameObject>($"../Uno/0Common/Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // 로드한 원본의 복사본을 만들어서 씬에 배치(copy)
        GameObject go = Object.Instantiate(original, parent);
        // 생성되는 GameObject에 (Clone) 붙는 거 없애주기
        go.name = original.name;

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}