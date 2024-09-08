using System.Collections.Generic;
using JazzyLucas.Core;
using JazzyLucas.Core.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core.Input
{
    public sealed class InputPoller
    {
        private readonly InputActions _inputActions;
        private Dictionary<string, float> _lastInputTimes;
        private readonly float _debounceDuration = 0.2f; // Debounce time in seconds
        public bool Enabled { get; private set; }

        // Constructor
        public InputPoller()
        {
            _inputActions = new();
            Enable();
        }

        public void Enable()
        {
            _inputActions.Enable();
            Enabled = true;
        }

        public void Disable()
        {
            _inputActions.Disable();
            Enabled = false;
        }

        public InputData PollInput()
        {
            if (!Enabled)
            {
                L.Log($"PollInput was called on an InputPoller than is not enabled.");
            }
            
            InputData data = new();

            var wasdCache = _inputActions.Player.WASD.ReadValue<Vector2>();
            data.WASD = new(Mathf.Clamp(wasdCache.x, -1, 1), Mathf.Clamp(wasdCache.y, -1, 1));
            data.MouseDelta = _inputActions.Player.MouseDelta.ReadValue<Vector2>();
            data.LeftClick = _inputActions.Player.LeftClick.triggered;
            data.RightClick = _inputActions.Player.RightClick.triggered;
            data.Shift = _inputActions.Player.Shift.IsPressed();
            data.Ctrl = _inputActions.Player.Ctrl.IsPressed();
            data.Spacebar = _inputActions.Player.Spacebar.IsPressed();
            data.PauseEscape = _inputActions.Player.PauseEscape.triggered;
            data.Q = _inputActions.Player.Q.triggered;
            data.E = _inputActions.Player.E.triggered;
            data.R = _inputActions.Player.R.triggered;
            data.F = _inputActions.Player.F.triggered;
            data.C = _inputActions.Player.C.triggered;
            if (_inputActions.Player.NumKey.triggered)
                data.NumKey = (int)_inputActions.Player.NumKey.ReadValue<float>();
            data.Scroll = Mathf.Clamp(_inputActions.Player.Scroll.ReadValue<float>(), -1, 1);

            return data;
        }
    }

    public struct InputData
    {
        public Vector2 WASD;
        public Vector2 MouseDelta;
        public float Scroll;
        
        // Mouse
        public bool LeftClick;
        public bool RightClick;
        
        // NumKeys
        public int NumKey;
        
        // Keys
        public bool PauseEscape;
        public bool Shift;
        public bool Ctrl;
        public bool Spacebar;
        public bool Q;
        public bool E;
        public bool R;
        public bool F;
        public bool C;
    }
    
    // TODO: use this when you make a hotbar controller
    public struct HotbarInputData
    {
        public static HotbarInputData GetFromInputData(InputData inputData)
        {
            return new()
            {
                HotbarSelectNum = inputData.NumKey-1,
                HotbarSelectUp = inputData.Scroll < 0,
                HotbarSelectDown = inputData.Scroll > 0,
            };
        }
        public int HotbarSelectNum { get; private set; }
        public bool HotbarSelectUp { get; private set; }
        public bool HotbarSelectDown { get; private set; }
    }
}