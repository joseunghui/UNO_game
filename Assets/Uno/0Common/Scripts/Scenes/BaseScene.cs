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

        StartCoroutine(BackendInitialize());
    }

    IEnumerator BackendInitialize()
    {
        var bro = Backend.Initialize(true); // �ڳ� �ʱ�ȭ

        // �ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            Debug.Log("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 204 Success
        }
        else
        {
            Debug.LogError("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 400�� ���� �߻�
            yield break;
        }
    }

    private void Update()
    {
        Backend.AsyncPoll();
    }

    public abstract void Clear();
}