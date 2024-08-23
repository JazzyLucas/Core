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
        [field: HideInInspector] public HitscanReceiver CurrentHitscan => CurrentHitscanGO == null ? null : CurrentHitscanGO.GetComponent<HitscanReceiver>();

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
            Ray.RaycastHitObject(out var go, Transform);

            CurrentHitscanGO = go;

            if (CurrentHitscanGO == null)
                return;
            
            var input = inputPoller.PollInput();
            
            var data = HitscanData.GetFromInputData(input);
            
            if (CurrentHitscan != null)
                CurrentHitscan.InvokeOnHitscan(data);
            
            //L.Log($"{raycastedOnGB.name} raycasted on! --- Type:{type} --- Mod:{mod}");
        }
    }
    
    [System.Serializable]
    public struct HitscanData
    {
        public HitscanHoldType HitscanHoldType { get; private set; }
        public HitscanClickType HitscanClickType { get; private set; }
        public InputData UsedInputData { get; private set; }
        public static HitscanData GetFromInputData(InputData inputData)
        {
            var holdType = HitscanHoldType.NONE;
            if (inputData.Shift)
                holdType = HitscanHoldType.HOLDING_SHIFT;
            else if (inputData.Ctrl)
                holdType = HitscanHoldType.HOLDING_CTRL;
            else if (!(inputData.LeftClick || inputData.RightClick) && inputData.LeftClickHold || inputData.RightClickHold)
                holdType = HitscanHoldType.HOLDING_CLICK;

            var clickType = HitscanClickType.NONE;
            if (inputData.LeftClick || inputData.RightClick)
            {
                if (inputData.LeftClick)
                {
                    clickType = HitscanClickType.PRIMARY;
                }
                if (inputData.RightClick)
                {
                    clickType = HitscanClickType.SECONDARY;
                }
            }
            
            return new()
            {
                HitscanHoldType = holdType,
                HitscanClickType = clickType,
                UsedInputData = inputData,
            };
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
