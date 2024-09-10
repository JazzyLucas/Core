using System;
using UnityEngine;
using JazzyLucas.Core.Input;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    public class MovementController : Controller
    {
        [field: SerializeField] public Transform DirectionTransform { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        private Transform Transform => CharacterController.transform;

        [field: SerializeField] public float walkSpeed { get; private set; } = 4f;
        [field: SerializeField] public float runSpeed { get; private set; } = 6f;
        [field: SerializeField] public float jumpForce { get; private set; } = 0.8f;
        [field: SerializeField] public float flySpeed { get; private set; } = 12f;

        [field: Header("Debugging")]
        [field: SerializeField] public bool ShowDebugInOnGUI { get; private set; } = false;

        [field: HideInInspector] public Vector3 Direction => lastMovementDirection;
        [field: HideInInspector] public bool IsWalking { get; private set; }
        [field: HideInInspector] public bool IsRunning { get; private set; }
        [field: HideInInspector] public bool IsJumping { get; private set; }
        [field: HideInInspector] public bool IsInAir { get; private set; }
        [field: HideInInspector] public bool IsFlying { get; private set; }
        [field: HideInInspector] public bool IsStandingStill { get; private set; }

        private MovementState currentState;

        private Vector3 velocity = Vector3.zero;
        private Vector3 lastMovementDirection = Vector3.zero; // Persist the last movement direction

        private void OnGUI()
        {
            if (!ShowDebugInOnGUI)
                return;
            
            GUIStyle labelStyle = new(GUI.skin.label)
            {
                fontSize = 20,
                normal = new() { textColor = Color.white }
            };
            GUI.Label(new(10, 10, 300, 25), $"IsWalking: {IsWalking}", labelStyle);
            GUI.Label(new(10, 40, 300, 25), $"IsRunning: {IsRunning}", labelStyle);
            GUI.Label(new(10, 70, 300, 25), $"IsJumping: {IsInAir}", labelStyle);
            GUI.Label(new(10, 100, 300, 25), $"IsFlying: {IsFlying}", labelStyle);
            GUI.Label(new(10, 130, 300, 25), $"IsStandingStill: {IsStandingStill}", labelStyle);
        }

        protected override void Process()
        {
            HandleInput();
            HandleRotation();
            Debug.DrawRay(Transform.position, lastMovementDirection * 0.2f, Color.green);

            UpdateStateBooleans();
        }

        private void HandleInput()
        {
            var input = inputPoller.PollInput();
            
            var movementData = MovementInputData.GetFromInputData(input);

            if (movementData.toggleFlying)
                ToggleFlying();

            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (currentState)
            {
                case MovementState.Grounded:
                    HandleGroundedMovement(movementData);
                    break;
                case MovementState.Flying:
                    HandleFlyingMovement(movementData);
                    break;
            }
        }

        private void ToggleFlying()
        {
            currentState = currentState == MovementState.Flying ? MovementState.Grounded : MovementState.Flying;
            if (currentState == MovementState.Grounded)
            {
                velocity.y = 0f; // Reset vertical velocity when transitioning to grounded state.
            }
        }

        private void HandleGroundedMovement(MovementInputData input)
        {
            var moveDirection = CalculateMovementDirection(input) * GetCurrentSpeed(input.isSprinting);

            if (CharacterController.isGrounded)
            {
                velocity.y = -2f; // Ensure the player sticks to the ground.
                if (input.isJumping)
                {
                    velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
                    IsInAir = true; // Player is jumping
                }
                else
                {
                    IsInAir = false;
                }
            }
            else
            {
                velocity.y += Physics.gravity.y * Time.deltaTime;
            }

            CharacterController.Move((moveDirection + velocity) * Time.deltaTime);

            // Update last movement direction for debugging purposes
            if (moveDirection != Vector3.zero)
            {
                lastMovementDirection = moveDirection;
            }
        }

        private void HandleFlyingMovement(MovementInputData input)
        {
            var moveDirection = CalculateMovementDirection(input) * GetCurrentSpeed(input.isSprinting);

            float verticalMovement = 0f;
            if (input.isJumping) verticalMovement = flySpeed;
            if (input.isCrouching) verticalMovement = -flySpeed;

            velocity.y = verticalMovement;

            CharacterController.Move((moveDirection + Vector3.up * velocity.y) * Time.deltaTime);

            // Update last movement direction for debugging purposes
            if (moveDirection != Vector3.zero)
            {
                lastMovementDirection = moveDirection;
            }
        }

        private Vector3 CalculateMovementDirection(MovementInputData input)
        {
            // Get forward and right directions from the camera, but ignore the y component to lock to XZ plane
            var forwardMovement = Vector3.ProjectOnPlane(DirectionTransform.forward, Vector3.up).normalized * input.moveInput.y;
            var rightMovement = Vector3.ProjectOnPlane(DirectionTransform.right, Vector3.up).normalized * input.moveInput.x;

            // Combine and normalize the movement direction
            var moveDirection = (forwardMovement + rightMovement).normalized;

            return moveDirection;
        }

        private void HandleRotation()
        {
            var targetRotation = Quaternion.Euler(0, DirectionTransform.eulerAngles.y, 0);
            Transform.rotation = targetRotation;
        }

        private float GetCurrentSpeed(bool isSprinting)
        {
            return isSprinting ? runSpeed : walkSpeed;
        }

        private void UpdateStateBooleans()
        {
            var input = inputPoller.PollInput();
            
            // Crunch some info
            var movementData = MovementInputData.GetFromInputData(input);
            bool isMoving = movementData.moveInput != Vector2.zero;
    
            // Update the state booleans
            IsWalking = isMoving && !movementData.isSprinting && currentState == MovementState.Grounded;
            IsRunning = isMoving && movementData.isSprinting && currentState == MovementState.Grounded;
            IsFlying = currentState == MovementState.Flying;
            IsJumping = movementData.isJumping;
            IsInAir = !CharacterController.isGrounded;
            IsStandingStill = !isMoving && currentState == MovementState.Grounded;
        }
    }

    public struct MovementInputData
    {
        public static MovementInputData GetFromInputData(InputData inputData)
        {
            return new()
            {
                moveInput = inputData.WASD,
                isSprinting = inputData.Shift == InputState.Held,
                isJumping = inputData.Spacebar == InputState.Held,
                isCrouching = inputData.Ctrl == InputState.Held,
                toggleFlying = inputData.F == InputState.Pressed,
            };
        }
        public Vector2 moveInput { get; private set; }
        public bool isSprinting { get; private set; }
        public bool isJumping { get; private set; }
        public bool isCrouching { get; private set; }
        public bool toggleFlying { get; private set; }
    }

    public enum MovementState
    {
        Grounded,
        Flying
    }
}
