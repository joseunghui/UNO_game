using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_Entity : UI_Scene
{
    public List<Item> items;
    public Item item;
    public SpriteRenderer entity;
    public SpriteRenderer image;

    public int _num;
    public string _color;
    public Sprite _sprite;
    public Vector3 _originPos;

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
        _num = _item.num;
        _color = _item.color;

        item = _item;
        image.sprite = item.sprite;
    }
    public void MoveTransform(Vector3 pos, bool useDotween, float dotweenTime)
    {
        if (useDotween)
            transform.DOMove(pos, dotweenTime);
        else
            transform.position = pos;
    }


}
