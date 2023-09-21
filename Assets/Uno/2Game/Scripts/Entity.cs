using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Entity : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] SpriteRenderer entity;
    [SerializeField] SpriteRenderer image;

    public int num;
    public string color;
    public Sprite sprite;
    public Vector3 originPos;

    public void Setup(Item item){
        num = item.num;
        color = item.color;

        this.item = item;
        image.sprite = this.item.sprite;
    }
    public void MoveTransform(Vector3 pos, bool useDotween, float dotweenTime = 0){
        if(useDotween)
            transform.DOMove(pos, dotweenTime);
        else
            transform.position = pos;
    }
}
