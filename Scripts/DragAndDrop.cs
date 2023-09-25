using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Canvas canvas;
    [SerializeField] Sprite innerSprite, outerSprite;
    [SerializeField] Image image;

    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    Rigidbody2D rb;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        rb = GetComponent<Rigidbody2D>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    void FixedUpdate()
    {
        if(rb.bodyType == RigidbodyType2D.Dynamic && rb.position.y < -3.5) rb.bodyType= RigidbodyType2D.Static;
        if (rb.position.x > -1.3)
        {
            HandleSprites(1);
            rectTransform.localScale = new(2, 2, 2);
        }
        else
        {
            HandleSprites(0);
            rectTransform.localScale = new(1, 1, 1);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        canvasGroup.blocksRaycasts = false;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void HandleSprites(int table)
    {
        if(table == 0) image.sprite = outerSprite;
        else image.sprite = innerSprite;

    }
}
