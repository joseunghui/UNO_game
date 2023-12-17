using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    // 현재 씬 타입을 알아야 하는 일이 생기니까 
    // 객체 자체는 public 으로, Set은 protected로 선언해주기(자식 스크립트에서 접근 가능)
    public Define.Scene ScenType { get; protected set; } = Define.Scene.UnKnown; // 초기 설정 UnKnown


    void Awake()
    {
        init();
    }

    protected virtual void init()
    {
        // EventSystem 없으면 생성하는 로직
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));

        if (obj == null)
        {
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
        }
    }

    public abstract void Clear();
}