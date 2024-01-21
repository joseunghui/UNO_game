using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    int _originOrder;

    public void SetOriginOrder(int originOrder)
    {
        this._originOrder = originOrder;
        SetOrder(originOrder);
    }

    public void SetMostFrontOrder(bool isMostFront)
    {
        SetOrder(isMostFront ? 500 : _originOrder);
    }

    public void SetOrder(int order)
    {
        int mulOrder = order * 10;
        var cardImage = Utill.FindChild<SpriteRenderer>(gameObject);
        cardImage.sortingLayerName = "UI";
        cardImage.sortingOrder = mulOrder;
        
    }
}
