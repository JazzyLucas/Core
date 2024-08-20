using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using UnityEngine;

public class TooltipTest : MonoBehaviour
{
    private TooltipsManager tooltipsManager => (TooltipsManager)TooltipsManager.Instance;
    private TooltipsContainer tooltipsContainer => tooltipsManager.Container;

    [field: SerializeField] public Transform tooltipPoint { get; private set; }

    private void Start()
    {
        var tooltip = tooltipsManager.CreateTextTooltip(tooltipPoint);
    }
}
