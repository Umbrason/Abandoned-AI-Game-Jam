using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TooltipManager : MonoBehaviour
    {
        public static TooltipManager Singleton;
        public Tooltip tooltip;

        void Start()
        {
            Singleton = this;
        }

        public void Show(string headerContent = "", string bodyContent = "")
        {
            tooltip.gameObject.SetActive(true);
            tooltip.header.text = headerContent;
            tooltip.body.text = bodyContent;
            tooltip.layoutElement.enabled = tooltip.body.preferredWidth > 300 ? true : false;
        }

        public void Hide()
        {
            if (tooltip == null)
                return;
            if (tooltip.layoutElement != null)
                tooltip.layoutElement.enabled = false;
            tooltip.gameObject.SetActive(false);
        }
    }
}