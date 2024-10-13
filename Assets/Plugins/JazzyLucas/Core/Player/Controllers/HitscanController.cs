using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core.Input;
using JazzyLucas.Core.Utils;
using UnityEngine;
using L = JazzyLucas.Core.Utils.Logger;
using Ray = JazzyLucas.Core.Utils.RaycastUtils;

namespace JazzyLucas.Core
{
    public class HitscanController : Controller
    {
        [field: SerializeField] public Transform Transform { get; private set; }
        [field: SerializeField] public LayerMask LayerMask { get; private set; } = ~0;

        [field: HideInInspector] public GameObject CurrentHitscanGO { get; private set; }
        [field: HideInInspector] public HitscanReceiver CurrentHitscan { get; private set; }

        protected override void Process()
        {
            var input = inputPoller.PollInput();
            
            Ray.RaycastHitObject(out var newHitscanGO, Transform, LayerMask);

            if (newHitscanGO != CurrentHitscanGO)
            {
                HandleHitscanGOChange(newHitscanGO);
            }

            CurrentHitscan?.InvokeOnHitscan(input);
        }
        
        private void HandleHitscanGOChange(GameObject newHitscanGO)
        {
            CurrentHitscan?.InvokeOnUnHitscan();

            CurrentHitscanGO = newHitscanGO;
            CurrentHitscan = CurrentHitscanGO?.GetComponent<HitscanReceiver>();
        }
    }
}
