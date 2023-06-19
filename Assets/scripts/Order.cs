using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    [SerializeField] Renderer[] backRenderers;
    [SerializeField] Renderer[] middleRenderers;
    [SerializeField] string sortingLayerName;
    int originOrder;

    private void Start(){
        SetOrder(0);
    }
    
    public void SetOriginOrder(int originOrder){
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }

    public void SetMostFrontOrder(bool isMostFront){
        SetOrder(isMostFront ? 500 : originOrder);
    }
    
    public void SetOrder(int order){
        int mulOrder = order * 10;

        foreach(var renderer in backRenderers){
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder;
        }

        foreach(var renderer in middleRenderers){
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder + 1;
        }
    }
}
