using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzyLucas.Core
{
    [CreateAssetMenu(fileName = "TooltipsContainer", menuName = "JazzyLucas.Core/Container/TooltipsContainer")]
    public class TooltipsContainer : Container
    {
        [field: SerializeField] public TextTooltip TextTooltipPrefab { get; private set; }
        // (later add more tooltips here)
    }
}
