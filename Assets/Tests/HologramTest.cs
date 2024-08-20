using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using UnityEngine;

public class HologramTest : MonoBehaviour
{
    private HologramsManager hologramsManager => (HologramsManager)HologramsManager.Instance;
    private HologramsContainer hologramsContainer => hologramsManager.Container;

    [field: SerializeField] public Transform hologramPoint { get; private set; }
    
    [field: HideInInspector] private TextHologram textHologram { get; set; }

    private void Start()
    {
        textHologram = hologramsManager.CreateTextHologram(hologramPoint);
    }
}
