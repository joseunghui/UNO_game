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
        // ������ �޸𸮿� �ε�(original)
        GameObject original = Load<GameObject>($"../Uno/0Common/Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // �ε��� ������ ���纻�� ���� ���� ��ġ(copy)
        GameObject go = Object.Instantiate(original, parent);
        // �����Ǵ� GameObject�� (Clone) �ٴ� �� �����ֱ�
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