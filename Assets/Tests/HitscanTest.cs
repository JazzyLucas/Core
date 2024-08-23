using System;
using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using UnityEngine;

public class HitscanTest : MonoBehaviour
{
    [field: SerializeField] public HitscanReceiver Receiver { get; private set; }
    [field: SerializeField] public Renderer Renderer { get; private set; }
    [field: SerializeField] public Material HoveredMat { get; private set; }
    [field: SerializeField] public Material ClickedMat { get; private set; }
    
    [field: HideInInspector] public Material NormalMat { get; private set; }

    private void Awake()
    {
        NormalMat = Renderer.material;

        Receiver.OnHitscan += (data) =>
        {
            Renderer.material = HoveredMat;
            if (data.HitscanClickType != HitscanClickType.NONE)
                Renderer.material = ClickedMat;
        };
    }
}
