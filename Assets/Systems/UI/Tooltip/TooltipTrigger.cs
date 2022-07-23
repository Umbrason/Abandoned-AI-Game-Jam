using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Coroutine coroutine;
        public const float tooltipDelay = .2f;

        public string tooltipHeader, tooltipBody;

        public void OnPointerEnter(PointerEventData e)
        {
            coroutine = StartCoroutine(ShowTooltip());
        }

        public void OnPointerExit(PointerEventData e)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                TooltipManager.Singleton.Hide();
            }
        }

        public void OnDisable()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                TooltipManager.Singleton.Hide();
            }
        }

        private IEnumerator ShowTooltip()
        {
            float t = tooltipDelay;
            Vector3 mousePos = Input.mousePosition;
            while (t > 0)
            {
                t -= Time.deltaTime;
                if (Vector3.Distance(mousePos, Input.mousePosition) > 5f)
                {
                    t = tooltipDelay;
                    mousePos = Input.mousePosition;
                }
                yield return null;
            }

            TooltipManager.Singleton.Show(tooltipHeader, tooltipBody);
        }
    }
}
