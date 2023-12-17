using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    // ���� �� Ÿ���� �˾ƾ� �ϴ� ���� ����ϱ� 
    // ��ü ��ü�� public ����, Set�� protected�� �������ֱ�(�ڽ� ��ũ��Ʈ���� ���� ����)
    public Define.Scene ScenType { get; protected set; } = Define.Scene.UnKnown; // �ʱ� ���� UnKnown


    void Awake()
    {
        init();
    }

    protected virtual void init()
    {
        // EventSystem ������ �����ϴ� ����
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));

        if (obj == null)
        {
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
        }
    }

    public abstract void Clear();
}