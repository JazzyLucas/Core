using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JazzyLucas.Core
{
    public class TextTooltip : Tooltip
    {
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
    }
}
