using JazzyLucas.Core;
using JazzyLucas.Core.Input;
using UnityEngine;

namespace JazzyLucas.Core.Input
{
    public abstract class InputPoller
    {
        private readonly InputActions _inputActions;

        // Constructor
        protected InputPoller()
        {
            _inputActions = new();
            Disable();
        }

        public void Enable() => _inputActions.Enable();
        public void Disable() => _inputActions.Disable();

        public InputData PollInput()
        {
            InputData data = new();

            var wasdCache = _inputActions.Player.WASD.ReadValue<Vector2>();
            data.WASD = new(Mathf.Clamp(wasdCache.x, -1, 1), Mathf.Clamp(wasdCache.y, -1, 1));
            data.MouseDelta = _inputActions.Player.MouseDelta.ReadValue<Vector2>();
            data.LeftClick = _inputActions.Player.LeftClick.triggered;
            data.LeftClickHold = _inputActions.Player.LeftClick.IsPressed();
            data.RightClick = _inputActions.Player.RightClick.triggered;
            data.RightClickHold = _inputActions.Player.RightClick.IsPressed();
            data.Shift = _inputActions.Player.Shift.IsPressed();
            data.Ctrl = _inputActions.Player.Ctrl.IsPressed();
            data.Spacebar = _inputActions.Player.Spacebar.IsPressed();
            data.PauseEscape = _inputActions.Player.PauseEscape.triggered;
            data.Q = _inputActions.Player.Q.triggered;
            data.E = _inputActions.Player.E.triggered;
            data.F = _inputActions.Player.F.triggered;
            data.C = _inputActions.Player.C.triggered;
            data.R = _inputActions.Player.R.triggered;
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
        public int NumKey;
        public bool LeftClick, LeftClickHold, RightClick, RightClickHold, Shift, Ctrl, Spacebar, PauseEscape, Q, E, F, C, R;
    }
    public struct MovementInputData
    {
        public static MovementInputData GetFromPlayerInputStruct(InputData inputData)
        {
            return new()
            {
                moveInput = inputData.WASD,
                isSprinting = inputData.Shift,
                isJumping = inputData.Spacebar,
                isCrouching = inputData.Ctrl,
                toggleFlying = inputData.F,
            };
        }
        public Vector2 moveInput { get; private set; }
        public bool isSprinting { get; private set; }
        public bool isJumping { get; private set; }
        public bool isCrouching { get; private set; }
        public bool toggleFlying { get; private set; }
    }
    public struct DisplayInputData
    {
        public static DisplayInputData GetFromPlayerInputStruct(InputData inputData)
        {
            return new()
            {
                TogglePauseOrEscape = inputData.PauseEscape,
                ToggleShopMenu = inputData.C,
                CycleHotbar = inputData.Q,
                HotbarSelectNum = inputData.NumKey-1,
                HotbarSelectUp = inputData.Scroll < 0,
                HotbarSelectDown = inputData.Scroll > 0,
            };
        }
        public bool TogglePauseOrEscape { get; private set; }
        public bool CycleHotbar { get; private set; }
        public bool ToggleShopMenu { get; private set; }
        public int HotbarSelectNum { get; private set; }
        public bool HotbarSelectUp { get; private set; }
        public bool HotbarSelectDown { get; private set; }
    }
}