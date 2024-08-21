using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    private TooltipsManager tooltipsManager => (TooltipsManager)TooltipsManager.Instance;
    private TooltipsContainer tooltipsContainer => tooltipsManager.Container;
    
    private TextTooltip activeTooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        activeTooltip = tooltipsManager.CreateTextTooltip();
        activeTooltip.Text.text = "Your tooltip text here.";
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (activeTooltip != null)
        {
            // Optionally update the tooltip content or position
        }
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
