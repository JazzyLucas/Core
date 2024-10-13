using System;
using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using JazzyLucas.Core.Input;
using UnityEngine;

namespace JazzyLucas.Sample
{
    public class HitscanTest : MonoBehaviour
    {
        [field: SerializeField] public HitscanReceiver Receiver { get; private set; }
        [field: SerializeField] public Renderer Renderer { get; private set; }
        [field: SerializeField] public Material HoveredMat { get; private set; }
        [field: SerializeField] public Material ClickingMat { get; private set; }
    
        [field: HideInInspector] public Material NormalMat { get; private set; }

        [field: HideInInspector] public bool IsHitscannedOn { get; private set; } = false;

        private void Awake()
        {
            NormalMat = Renderer.material;

            Receiver.OnHitscan += (data) =>
            {
                Renderer.material = HoveredMat;
                if (data.LeftClick == InputState.Held || data.RightClick == InputState.Held)
                    Renderer.material = ClickingMat;
            };
            Receiver.OnUnHitscan += () =>
            {
                Renderer.material = NormalMat;
            };
        }
    }
}
