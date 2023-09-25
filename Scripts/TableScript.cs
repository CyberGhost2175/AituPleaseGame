using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TableScript : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            eventData.pointerDrag.GetComponent<DragAndDrop>().HandleSprites(0);
            eventData.pointerDrag.GetComponent<RectTransform>().localScale = new(1, 1, 1);
        }
    }
}
