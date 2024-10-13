using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzyLucas.Core
{
    public class TooltipsManager : Manager<TooltipsContainer>
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }

        public override void Init()
        {
            InitializeCanvas();
        }

        public TextTooltip CreateTextTooltip()
        {
            var tooltip = Instantiate(Container.TextTooltipPrefab, Canvas.transform);
            return tooltip;
        }

        public GenericTooltip CreateCustomTooltip(GameObject customPrefab)
        {
            var tooltip = Instantiate(customPrefab, Canvas.transform);
            if (tooltip.GetComponent<GenericTooltip>() == null)
                tooltip.AddComponent<GenericTooltip>();
            return tooltip.GetComponent<GenericTooltip>();
        }

        public void ReturnTooltip(Tooltip tooltip)
        {
            Destroy(tooltip.gameObject);
        }

        private void InitializeCanvas()
        {
            Canvas.overrideSorting = true;
            Canvas.sortingOrder = 32767; // int max
        }
    }
}
