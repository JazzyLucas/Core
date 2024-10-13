using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JazzyLucas.Sample
{
    public class CustomTooltipTest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [field: SerializeField] public GameObject CustomTooltipPrefab { get; private set; }
        
        private TooltipsManager tooltipsManager => (TooltipsManager)TooltipsManager.Instance;
        private TooltipsContainer tooltipsContainer => tooltipsManager.Container;
        
        private Tooltip activeTooltip;

        public void OnPointerEnter(PointerEventData eventData)
        {
            activeTooltip = tooltipsManager.CreateCustomTooltip(CustomTooltipPrefab);
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