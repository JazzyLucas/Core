using System.Collections.Generic;
using JazzyLucas.Core;
using JazzyLucas.Core.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using L = JazzyLucas.Core.Utils.Logger;
using System;

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
            _lastInputTimes = new Dictionary<string, float>(); // Initialize the dictionary
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
            InputData data = new();

            // WASD remains the same (continuous input)
            var wasdCache = _inputActions.Player.WASD.ReadValue<Vector2>();
            data.WASD = new(Mathf.Clamp(wasdCache.x, -1, 1), Mathf.Clamp(wasdCache.y, -1, 1));

            // Mouse Delta
            data.MouseDelta = _inputActions.Player.MouseDelta.ReadValue<Vector2>();

            // LeftClick state
            data.LeftClick = GetInputState(_inputActions.Player.LeftClick);

            // RightClick state
            data.RightClick = GetInputState(_inputActions.Player.RightClick);

            // Shift state
            data.Shift = GetInputState(_inputActions.Player.Shift);

            // Ctrl state
            data.Ctrl = GetInputState(_inputActions.Player.Ctrl);

            // Spacebar state
            data.Spacebar = GetInputState(_inputActions.Player.Spacebar);

            // Other keys with debouncing
            data.PauseEscape = GetInputState(_inputActions.Player.PauseEscape);
            data.Q = GetInputState(_inputActions.Player.Q);
            data.E = GetInputState(_inputActions.Player.E);
            data.R = GetInputState(_inputActions.Player.R);
            data.F = GetInputState(_inputActions.Player.F);
            data.C = GetInputState(_inputActions.Player.C);

            // NumKey with debouncing
            if (_inputActions.Player.NumKey.triggered && CanProcessInput("NumKey", Time.time))
            {
                data.NumKey = (int)_inputActions.Player.NumKey.ReadValue<float>();
                UpdateLastInputTime("NumKey", Time.time);
            }

            // Scroll remains unchanged
            data.Scroll = Mathf.Clamp(_inputActions.Player.Scroll.ReadValue<float>(), -1, 1);

            return data;
        }


        // Helper method to check if input can be processed based on debouncing
        private bool CanProcessInput(string actionName, float currentTime)
        {
            if (!_lastInputTimes.TryGetValue(actionName, out float lastInputTime))
            {
                return true; // No previous input, allow processing
            }
            return (currentTime - lastInputTime) >= _debounceDuration;
        }

        // Helper method to update the last input time for an action
        private void UpdateLastInputTime(string actionName, float currentTime)
        {
            _lastInputTimes[actionName] = currentTime;
        }

        // Method to process debounced input for buttons
        private bool ProcessDebouncedInput(InputAction inputAction, string actionName, float currentTime)
        {
            if (inputAction.triggered && CanProcessInput(actionName, currentTime))
            {
                UpdateLastInputTime(actionName, currentTime);
                return true;
            }
            return false;
        }
        
        private static InputState GetInputState(InputAction inputAction)
        {
            if (inputAction.WasPressedThisFrame())
            {
                return InputState.Pressed;
            }
            if (inputAction.IsPressed())
            {
                return InputState.Held;
            }
            if (inputAction.WasReleasedThisFrame())
            {
                return InputState.Released;
            }
            return InputState.None;
        }
    }
    
    public enum InputState
    {
        None,
        Pressed,
        Held,
        Released
    }

    public struct InputData
    {
        public Vector2 WASD;
        public Vector2 MouseDelta;
        public float Scroll;

        // Mouse
        public InputState LeftClick;
        public InputState RightClick;

        // NumKeys
        public int NumKey;

        // Keys
        public InputState PauseEscape;
        public InputState Shift;
        public InputState Ctrl;
        public InputState Spacebar;
        public InputState Q;
        public InputState E;
        public InputState R;
        public InputState F;
        public InputState C;
    }

    // TODO: use this when you make a hotbar controller
    public struct HotbarInputData
    {
        public static HotbarInputData GetFromInputData(InputData inputData)
        {
            return new()
            {
                HotbarSelectNum = inputData.NumKey - 1,
                HotbarSelectUp = inputData.Scroll < 0,
                HotbarSelectDown = inputData.Scroll > 0,
            };
        }
        public int HotbarSelectNum { get; private set; }
        public bool HotbarSelectUp { get; private set; }
        public bool HotbarSelectDown { get; private set; }
    }
}
