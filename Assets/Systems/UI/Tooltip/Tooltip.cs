using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Tooltip : MonoBehaviour
    {
        public TextMeshProUGUI header;
        public TextMeshProUGUI body;
        public LayoutElement layoutElement;
        public RectTransform rectTransform;

        public void Update()
        {
            //float pivotX = Mathf.RoundToInt(Input.mousePosition.x / Screen.width * 2) / 2f;
            //float pivotY = Mathf.RoundToInt(Input.mousePosition.y / Screen.height * 2) / 2f;
            var overflowRight = Input.mousePosition.x + rectTransform.rect.width >= Screen.width - 5;
            var overflowTop = Input.mousePosition.y + rectTransform.rect.height >= Screen.height - 5;
            float pivotX = overflowRight ? 1 : 0;
            float pivotY = overflowTop ? 1 : 0;
            rectTransform.pivot = new Vector2(pivotX, pivotY);
            rectTransform.position = Input.mousePosition;
        }
    }
}