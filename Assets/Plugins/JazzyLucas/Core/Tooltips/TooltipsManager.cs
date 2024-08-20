using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzyLucas.Core
{
    public class TooltipsManager : Manager<TooltipsContainer>
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }

        [field: Header("(Camera can be retrieved from Camera.main)")]
        [field: SerializeField] public Camera MainCamera { get; private set; }

        public override void Init()
        {
            if (!MainCamera)
                MainCamera = Camera.main;

            // TODO: create a pool of Tooltips and use those
        }

        public TextTooltip CreateTextTooltip(Transform anchorTo)
        {
            var prefab = Container.TextTooltipPrefab;
            var newTooltipGO = Instantiate(prefab, Canvas.transform);
            var newTooltip = newTooltipGO.GetComponent<TextTooltip>();
            newTooltip.MainCamera = MainCamera;
            newTooltip.AnchorTo = anchorTo;
            return newTooltip;
        }
    }
}
