using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UICardToken : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CardBase card;
    public CardBase Card { get { return card; } }

    private Transform preDragParent;

    public Action<UICardToken> OnBeginDragCard;
    public Action<UICardToken> OnDragCard;
    public Action<UICardToken> OnEndDragCard;

    public Action<UICardToken> OnPointerEnterCard;
    public Action<UICardToken> OnPointerExitCard;

    private Canvas m_Canvas;
    public Canvas Canvas { get { return m_Canvas ??= GetComponentsInParent<Canvas>().Where((x) => x.isActiveAndEnabled).SingleOrDefault(); } }

    private RectTransform m_RectTransform;
    public RectTransform RectTransform { get { return m_RectTransform ??= GetComponent<RectTransform>(); } }    
    //Description

    public void SetCard(CardBase card)
    {
        this.card = card;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        preDragParent = transform.parent;
        transform.SetParent(Canvas.transform);
        OnBeginDragCard?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        OnDragCard?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(preDragParent);
        OnEndDragCard?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData) => OnPointerEnterCard?.Invoke(this);
    public void OnPointerExit(PointerEventData eventData) => OnPointerExitCard?.Invoke(this);
}