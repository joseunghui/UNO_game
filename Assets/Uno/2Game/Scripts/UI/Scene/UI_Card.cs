using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UI_Card : UI_Scene
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
        item.color = _item.color;
        item.num = _item.num;
        item.sprite = _item.sprite;
        this.isFront = isFront;
        
        var cardImage = Utill.FindChild<SpriteRenderer>(gameObject);
        Debug.Log($"카드 셋업 : {item.sprite}");
        
        if (this.isFront)
        {
            cardImage.sprite = this.item.sprite;
            Debug.Log($"{cardImage.sprite}");
        }   
    }

    void OnMouseOver()
    {
        //if (isFront)
            //cardController.CardMouseOver(this);
    }


    void OnMouseExit()
    {
        //if (isFront)
            //cardController.CardMouseExit(this);
    }
    void OnMouseDown()
    {
        //if (isFront)
            //cardController.CardMouseDown();
    }
    void OnMouseUp()
    {
        //if (isFront)
            //cardController.CardMouseUp();
    }

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
}

