using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CardLayoutProvider : LayoutGroup {
    public float preferredSpacing;
    [SerializeField] private int highlightedCard = -1;
    public int HighlightedCard { get { return highlightedCard; } set => SetHighlightedCard(value); }
    public float highlightedScaleUp = 0.3f;
    [SerializeField] private int placeholderPosition = -1;
    private bool PlaceholderActive { get { return placeholderPosition >= 0 && placeholderPosition <= rectChildren.Count; } }

    public new List<RectTransform> rectChildren { get { return (from i in Enumerable.Range(0, transform.childCount) select i).Select((i) => transform.GetChild(i)).Where((x) => x.gameObject.activeInHierarchy && x is RectTransform).Select((x) => x as RectTransform).ToList(); } }
    public int PlaceholderPosition { get { return placeholderPosition; } }
    public AnimationCurve heightCurve;
    public float heightMultiplier;
    public AnimationCurve rotationCurve;
    public float rotationMultiplier;
    private float PreferredHandWidth
    {
        get
        {
            var sum = rectChildren.Sum((x) => x.rect.width);
            var placeholderMask = PlaceholderActive ? 1 : 0;
            var final = sum * (1 + placeholderMask / Mathf.Max(1f, rectChildren.Count)) + preferredSpacing * (rectChildren.Count + placeholderMask - 1);
            return final;
        }
    }

    public override void CalculateLayoutInputVertical() => DoLayout();
    public override void SetLayoutHorizontal() => DoLayout();
    public override void SetLayoutVertical() => DoLayout();


    public void SetPlaceholderCard(int index) { placeholderPosition = index; DoLayout(); }
    public void ResetPlaceholderCard() { placeholderPosition = -1; DoLayout(); }

    public void SetHighlightedCard(int index) { highlightedCard = index; DoLayout(); }
    public void ResetHighlightedCard() { highlightedCard = -1; DoLayout(); }

    public void ChangeCardIndex(int oldIndex, int newIndex)
    {
        if (oldIndex < 0 || newIndex < 0 || oldIndex >= rectChildren.Count || newIndex > rectChildren.Count)
            return;
        highlightedCard = oldIndex == highlightedCard ? newIndex : highlightedCard;
        rectChildren[oldIndex].SetSiblingIndex(GetSiblingIndex(newIndex));
        DoLayout();
    }

    private int GetSiblingIndex(int cardIndex)
    {
        int siblingIndex = 0;
        while (siblingIndex < transform.childCount && cardIndex > 0)
        {
            var t = transform.GetChild(siblingIndex);
            if (t.gameObject.activeSelf)
                cardIndex--;
            siblingIndex++;
        }
        return siblingIndex;
    }

    public int CardIndexByPosition(float x)
    {
        var maxHandWidth = rectTransform.rect.width - padding.left - padding.right;
        var handWidth = Mathf.Min(maxHandWidth, PreferredHandWidth);

        for (int i = 0; i < rectChildren.Count; i++)
        {
            var placeholderMask = (PlaceholderActive && i >= placeholderPosition) ? 1 : 0;
            var highlightedMask = i == highlightedCard ? 1 : 0;
            var t = (i + placeholderMask + 0.5f) / (rectChildren.Count + (PlaceholderActive ? 1 : 0));
            var posX = handWidth * t - rectChildren[i].rect.width / 2f + padding.left + (maxHandWidth - handWidth) / 2f;
            var xOffset = highlightedCard >= 0 && highlightedCard < rectChildren.Count ? //mask out of bounds
                ((i > highlightedCard ? 1 : 0) + (i < highlightedCard ? -1 : 0))         //mask left/right/exactly highlighted
                * rectChildren[highlightedCard].rect.width * highlightedScaleUp / 2f     //scale mask to reflect highlighted size
                : 0;
            if (posX + xOffset + rectChildren[i].rect.width / 2f > x)
                return i;
        }
        return rectChildren.Count;
    }


    private void DoLayout()
    {
        var maxHandWidth = rectTransform.rect.width - padding.left - padding.right;
        var handWidth = Mathf.Min(maxHandWidth, PreferredHandWidth);
        var maxHandHeight = rectTransform.rect.height;
        var handHeight = rectTransform.rect.height;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            var placeholderMask = (PlaceholderActive && i >= placeholderPosition) ? 1 : 0;
            var highlightedMask = i == highlightedCard ? 1 : 0;
            var t = (i + placeholderMask + 0.5f) / (rectChildren.Count + (PlaceholderActive ? 1 : 0));

            //space along x
            var posX = handWidth * t - rectChildren[i].rect.width / 2f + padding.left + (maxHandWidth - handWidth) / 2f;
            var xOffset = highlightedCard >= 0 && highlightedCard < rectChildren.Count ? //mask out of bounds
                ((i > highlightedCard ? 1 : 0) + (i < highlightedCard ? -1 : 0))         //mask left/right/exactly highlighted
                * rectChildren[highlightedCard].rect.width * highlightedScaleUp / 2f     //scale mask to reflect highlighted size
                : 0;
            SetChildAlongAxis(rectChildren[i], 0, posX + xOffset);

            //move up/down
            var posY = heightCurve.Evaluate(t) * heightMultiplier;
            var yOffset = highlightedCard >= 0 && highlightedCard < rectChildren.Count ? //mask out of bounds
            highlightedMask *                                                            //mask highlighted
            rectChildren[highlightedCard].rect.height * highlightedScaleUp / 2f          //scale mask
             : 0;
            SetChildAlongAxis(rectChildren[i], 1, -(posY + yOffset));

            //rotate on Z        
            var rotation = rotationCurve.Evaluate(t) * rotationMultiplier * (t > 0.5 ? -1 : 1);
            rectChildren[i].rotation = Quaternion.Euler(Vector3.forward * rotation);
            rectChildren[i].localScale = Vector3.one * (1 + highlightedMask * highlightedScaleUp);
        }
    }
}
