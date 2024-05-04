using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Define.MouseEvent inputState; // 동작안함, 마우스오버, 누름(있어야 하나... 고민), 클릭, 더블클릭(막으려고 넣음), 드래그
    public Action MouseAction = null;

    bool putCardArea;
    bool _draggable = false;
    bool _pressed = false;
    public UI_Card selectCard;

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (GetCardController().gameState != Define.GameState.GameStart)
            return;
        SetECardState();

        if (_pressed)
        {
            CardDrag();
        }
    }

    #region MyCard
    public void SetMouseAction(Define.MouseEvent input)
    {
        if (GetCardController().gameState != Define.GameState.GameStart)
            return;
        inputState = input;

        switch (inputState)
        {
            case Define.MouseEvent.Over:
                {
                    MouseAction += OnMouseOver; break;
                }
            case Define.MouseEvent.Exit:
                {
                    MouseAction += OnMouseExit; break;
                }
            case Define.MouseEvent.Down:
                {
                    MouseAction += OnMouseDown; break;
                }
            case Define.MouseEvent.Up:
                {
                    MouseAction += OnMouseUp; break;
                }
            default: break;
        }
        MouseAction?.Invoke();
    }

    public void OnMouseOver()
    {
        if (selectCard == null)
            return;
        EnlargeCard(true, selectCard);
        Clear();
    }

    public void OnMouseExit()
    {
        if (selectCard == null)
            return;
        EnlargeCard(false, selectCard);
        Clear();
    }

    public void OnMouseDown()
    {
        if (_draggable)
        {
            Clear();
            _pressed = true;
        }
    }

    public void OnMouseUp()
    {
        if (!_pressed)
            return;
        _pressed = false;
        DetectCardArea();
        Clear();
        if (!putCardArea)
            GetCardController().CardAlignment(true);
        else
        {
            if (GetCardController().TryPutCard(true, selectCard))
            {
                //GetTurnController().EndTurn();
            }
            selectCard = null;
        }
    }

    void CardDrag()
    {
        selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard._originPRS.scale), false);
    }

    // 이거 고민해보기!!!
    void DetectCardArea()
    {
        /*if (Physics.Raycast(Utils.MousePos, Vector3.forward, Mathf.Infinity, LayerMask.GetMask("PutCardArea")))
            putCardArea = true;
        */
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("PutCardArea");
        putCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
        Debug.Log(putCardArea);
    }

    void EnlargeCard<T>(bool isEnlarge, T target)
    {
        var card = target as UI_Card;
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card._originPRS.pos.x, -2.1f, -5f);   // x는 그대로 y 올리고 z는 앞으로 뻄
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 1.4f), false);
        }
        else
            card.MoveTransform(card._originPRS, false);
    }
    void SetECardState()
    {
        if (GetCardController().myTurn && GetCardController().putCount == 0)    // 내 턴일땐 가능
            _draggable = true;
        else if (!GetCardController().myTurn || GetCardController().putCount == 1)   // 드래그 못하게
            _draggable = false;
    }
    #endregion

    public void Clear()
    {
        inputState = Define.MouseEvent.None;
        MouseAction = null;
    }

    CardController GetCardController()
    {
        return GameObject.FindWithTag("Scene").GetOrAddComponent<CardController>();
    }
}
