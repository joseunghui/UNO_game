using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDoubleClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;


    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

    public void OnPointerDoubleClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnDoubleClickHandler != null)
            OnDoubleClickHandler.Invoke(eventData);
    }

}
