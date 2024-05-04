using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class UI_Card : UI_SubItem
{
    public Item item;
    public PRS _originPRS;
    bool isFront;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
    }

    public void Setup(bool isFront, Item _item)
    {
        this.item.color = _item.color;
        this.item.num = _item.num;
        this.item.sprite = _item.sprite;
        this.isFront = isFront;
        
        var cardImage = Utill.FindChild<Image>(gameObject);
        
        if (this.isFront)
        {
            cardImage.sprite = this.item.sprite;
        }   
    }

    #region Mouse Action
    void OnMouseOver()
    {
        if (isFront) {
            Managers.Input.selectCard = this;
            Managers.Input.SetMouseAction(Define.MouseEvent.Over);
        }
    }
    void OnMouseExit()
    {
        if (isFront)
        {
            Managers.Input.selectCard = this;
            Managers.Input.SetMouseAction(Define.MouseEvent.Exit);
        }
    }
   void OnMouseDown()
    {
        if (isFront)
        {
            Managers.Input.selectCard = this;
            Managers.Input.SetMouseAction(Define.MouseEvent.Down);
        }
    }
    void OnMouseUp()
    {
        if (isFront)
            Managers.Input.SetMouseAction(Define.MouseEvent.Up);
    }
    #endregion
    #region Move Transform
    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
    #endregion
}

