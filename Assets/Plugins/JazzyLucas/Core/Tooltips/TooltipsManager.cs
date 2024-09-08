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
            
        }

        public TextTooltip CreateTextTooltip()
        {
            var tooltip = Instantiate(Container.TextTooltipPrefab, Canvas.transform);
            return tooltip;
        }

        public Tooltip CreateCustomTooltip(GameObject customPrefab)
        {
            var tooltip = Instantiate(customPrefab, Canvas.transform);
            if (tooltip.GetComponent<Tooltip>() == null)
                tooltip.AddComponent<Tooltip>();
            return tooltip.GetComponent<Tooltip>();
        }

        public void ReturnTooltip(Tooltip tooltip)
        {
            Destroy(tooltip.gameObject);
        }
    }
}
