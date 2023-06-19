using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Card : MonoBehaviour
{
    [SerializeField] Sprite cardBack;

    public Item item;
    public PRS originPRS;
    bool isFront;

    public void Setup(Item item, bool isFront){
        print(item.sprite);
        this.item = item;
        this.isFront = isFront;
        if(this.isFront)
            item.sprite = this.item.sprite;
        else
            item.sprite = cardBack;
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0){
        if(useDotween){
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        } else{
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
}
