using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_Entity : UI_Scene
{
    public Item item;
    public Vector3 _originPRS;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
    }

    public void Setup(Item _item)
    {
        this.item.color = _item.color;
        this.item.num = _item.num;
        this.item.sprite = _item.sprite;
        var cardImage = Utill.FindChild<SpriteRenderer>(gameObject);
        Debug.Log($"{item.sprite}");
        
        cardImage.sprite = _item.sprite;
        Debug.Log($"{cardImage.sprite}");
        
    }
    public void MoveTransform(Vector3 pos, bool useDotween, float dotweenTime)
    {
        if (useDotween)
            transform.DOMove(pos, dotweenTime);
        else
            transform.position = pos;
    }


}
