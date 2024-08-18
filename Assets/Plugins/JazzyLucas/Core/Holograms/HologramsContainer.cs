using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "HologramsContainer", menuName = "Container/HologramsContainer")]
public class HologramsContainer : Container
{
    [field: SerializeField] public TextHologram TextHologramPrefab { get; private set; }
    // (later add more holograms here)
}
