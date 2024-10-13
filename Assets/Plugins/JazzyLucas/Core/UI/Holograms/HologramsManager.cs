using System;
using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using UnityEngine;

namespace JazzyLucas.Core
{
    public class HologramsManager : Manager<HologramsContainer>
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }

        [field: Header("(Camera can be retrieved from Camera.main)")]
        [field: SerializeField] public Camera MainCamera { get; private set; }

        public override void Init()
        {
            if (!MainCamera)
                MainCamera = Camera.main;

            // TODO: create a pool of Holograms and use those
        }

        public TextHologram CreateTextHologram(Transform anchorTo)
        {
            var prefab = Container.TextHologramPrefab;
            var newHologramGO = Instantiate(prefab, Canvas.transform);
            var newHologram = newHologramGO.GetComponent<TextHologram>();
            newHologram.MainCamera = MainCamera;
            newHologram.AnchorTo = anchorTo;
            return newHologram;
        }
    }
}