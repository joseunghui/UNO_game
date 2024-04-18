using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager 
{
    public Action KeyAction = null;

    public Define.GameState gameState; // 게임 시작, 중단, 종료
    public Define.MouseEvent inputState; // 동작안함, 마우스오버, 누름(있어야 하나... 고민), 클릭, 더블클릭(막으려고 넣음), 드래그
    
    public Action<Define.GameState> GameAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;
    bool _draggable = false;

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;


        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        /**
         * 커서 위치 가져오기 : Vector3 mousePos = Input.mousePosition;
         * GetMouseButton(0) : 마우스 왼쪽
         * GetMouseButton(1) : 마우스 오른쪽
         * GetMouseButton(2) : 마우스 휠
         */
        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if (_pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.Click);
                }
                _pressed = false;
            }
        }
    }

    #region MyCard
    public void OnMouseOver<T>(T target)
    {
        if (inputState == Define.MouseEvent.None)
            return;
        EnlargeCard(true, target);
    }

    public void OnMouseExit<T>(T targer)
    {
        EnlargeCard(false, targer);
    }

    public void OnMouseDown()
    {
        if (inputState != Define.MouseEvent.Drag)
            return;
        _draggable = true;
    }

    public void OnMouseUp()
    {
        _draggable = false;
        
        if (inputState != Define.MouseEvent.Drag)
            return;

        if (true) // OnMyCardArea
            GetCardController().EntityAlignment();
        else
        {
            if (GetCardController().TryPutCard(true))
            {
                GetTurnController().EndTurn();
            }
        }
    }

    void CardDrag(UI_Card targetCard)
    {
        if (inputState != Define.MouseEvent.Drag)
            return;

        if (!true) // OnMyCardArea
        {
            targetCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, targetCard._originPRS.scale), false);
            GetCardController().EntityAlignment();
        }
    }

    // 이거 고민해보기!!!
    void DetectCardArea()
    {  
        // MyCardArea랑 마우스랑 겹치는 부분이 있으면 true
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        //OnMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    void EnlargeCard<T>(bool isEnlarge, T TargetCard)
    {
        UI_Card card = TargetCard as UI_Card;

        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card._originPRS.pos.x, -2.1f, -5f);   // x는 그대로 y 올리고 z는 앞으로 뻄
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 1.4f), false);
        }
        else
            card.MoveTransform(card._originPRS, false);

        card.GetComponent<OrderController>().SetMostFrontOrder(isEnlarge);
    }
    void SetECardState()
    {
        if (GetTurnController().isLoading)  // 게임 로딩중일땐 아무것도 안되고
            inputState = Define.MouseEvent.None;
        else if (!GetTurnController().myTurn || GetCardController().putCount == 1)   // 드래그 못하게
            inputState = Define.MouseEvent.Over;
        else if (GetTurnController().myTurn && GetCardController().putCount == 0)    // 내 턴일땐 가능
            inputState = Define.MouseEvent.Drag;

    }
    #endregion

    public void Clear()
    {
        KeyAction = null;
        GameAction = null;
        MouseAction = null;
    }

    TurnController GetTurnController()
    {
        return GameObject.FindWithTag("Scene").GetOrAddComponent<TurnController>();
    }

    CardController GetCardController()
    {
        return GameObject.FindWithTag("Scene").GetOrAddComponent<CardController>();
    }
}
