using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JazzyLucas.Sample
{
    public class TextTooltipTest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private TooltipsManager tooltipsManager => (TooltipsManager)TooltipsManager.Instance;
        private TooltipsContainer tooltipsContainer => tooltipsManager.Container;
    
        private TextTooltip activeTooltip;

        public void OnPointerEnter(PointerEventData eventData)
        {
            activeTooltip = tooltipsManager.CreateTextTooltip();
            activeTooltip.Text.text = "Your tooltip text here.";
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (activeTooltip != null)
            {
                tooltipsManager.ReturnTooltip(activeTooltip);
                activeTooltip = null;
            }
        }
    }
}
