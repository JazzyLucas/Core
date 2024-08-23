using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core.Input;
using JazzyLucas.Core.Utils;
using UnityEngine;
using L = JazzyLucas.Core.Utils.Logger;
using Ray = JazzyLucas.Core.Utils.RaycastUtils;

namespace JazzyLucas.Core
{
    public class HitscanController : MonoBehaviour
    {
        [field: SerializeField] public Transform Transform { get; private set; }
        [field: SerializeField] public LayerMask LayerMask { get; private set; } = ~0;

        [field: HideInInspector] public GameObject CurrentHitscanGO { get; private set; }
        [field: HideInInspector] public HitscanReceiver CurrentHitscan { get; private set; }

        private InputPoller inputPoller;

        private void Awake()
        {
            inputPoller = new();
        }
        private void Update()
        {
            Process();
        }

        public void Process()
        {
            Ray.RaycastHitObject(out var newHitscanGO, Transform, LayerMask);

            if (newHitscanGO != CurrentHitscanGO)
            {
                HandleHitscanChange(newHitscanGO);
            }

            if (CurrentHitscan != null)
            {
                ProcessHitscan();
            }
        }
        
        private void HandleHitscanChange(GameObject newHitscanGO)
        {
            CurrentHitscan?.InvokeOnUnHitscan();

            CurrentHitscanGO = newHitscanGO;
            CurrentHitscan = CurrentHitscanGO?.GetComponent<HitscanReceiver>();

            if (CurrentHitscan != null)
            {
                ProcessHitscan();
            }
        }

        private void ProcessHitscan()
        {
            var input = inputPoller.PollInput();
            CurrentHitscan.InvokeOnHitscan(input);
        }
    }
    
    public enum HitscanClickType
    {
        NONE,
        PRIMARY, // Left click
        SECONDARY, // Right click
    }
    public enum HitscanHoldType
    {
        NONE,
        HOLDING_CLICK, // Holding the click
        HOLDING_SHIFT, // Holding shift
        HOLDING_CTRL, // Holding ctrl
    }
}
