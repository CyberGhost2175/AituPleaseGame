using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BigTableScript : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<DragAndDrop>().HandleSprites(1);
            eventData.pointerDrag.GetComponent<RectTransform>().localScale = new (2, 2, 2);
        }
    }    
}
