using System;
using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using UnityEngine;

namespace JazzyLucas.Sample
{
    public class HoverableTest : MonoBehaviour
    {
        [field: SerializeField] public Hoverable hoverable { get; private set; }

        private void Awake()
        {
            hoverable.OnHover += () =>
            {
                Debug.Log("Hovered");
            };
            hoverable.OnUnhover += () =>
            {
                Debug.Log("Unhovered");
            };
        }
    }
}