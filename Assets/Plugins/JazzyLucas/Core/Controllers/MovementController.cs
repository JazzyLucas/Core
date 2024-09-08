using System;
using JazzyLucas.Core.Input;
using UnityEngine;

namespace JazzyLucas.Core
{
    public class MovementController : MonoBehaviour
    {
        [field: SerializeField] public Transform DirectionTransform { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        private Transform Transform => CharacterController.transform;

        private InputPoller inputPoller;
        private MovementState currentState;

        [field: SerializeField] public float walkSpeed { get; private set; } = 4f;
        [field: SerializeField] public float runSpeed { get; private set; } = 6f;
        [field: SerializeField] public float jumpForce { get; private set; } = 0.8f;
        [field: SerializeField] public float flySpeed { get; private set; } = 12f;

        private Vector3 velocity = Vector3.zero;
        private Vector3 lastMovementDirection = Vector3.zero; // Persist the last movement direction

        // New state booleans
        public bool IsWalking { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsFlying { get; private set; }
        public bool IsStandingStill => lastMovementDirection == Vector3.zero && CharacterController.isGrounded;

        private void Awake()
        {
            inputPoller = new();
        }

        private void Update()
        {
            var input = inputPoller.PollInput();
            HandleInput(input);
            HandleRotation();
            Debug.DrawRay(Transform.position, lastMovementDirection * 0.2f, Color.green);

            UpdateStateBooleans(); // Update booleans for state checking
        }

        private void OnGUI()
        {
            // Define the style for the labels
            GUIStyle labelStyle = new(GUI.skin.label)
            {
                fontSize = 20,
                normal = new() { textColor = Color.white }
            };
            
            // Display the boolean values
            GUI.Label(new(10, 10, 300, 25), $"IsWalking: {IsWalking}", labelStyle);
            GUI.Label(new(10, 40, 300, 25), $"IsRunning: {IsRunning}", labelStyle);
            GUI.Label(new(10, 70, 300, 25), $"IsJumping: {IsJumping}", labelStyle);
            GUI.Label(new(10, 100, 300, 25), $"IsFlying: {IsFlying}", labelStyle);
            GUI.Label(new(10, 130, 300, 25), $"IsStandingStill: {IsStandingStill}", labelStyle);
        }

        private void HandleInput(InputData input)
        {
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
                    IsJumping = true; // Player is jumping
                }
                else
                {
                    IsJumping = false;
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

        // Update state booleans based on current inputs and conditions
        private void UpdateStateBooleans()
        {
            IsWalking = lastMovementDirection != Vector3.zero && !IsRunning && !IsFlying && CharacterController.isGrounded;
            IsRunning = lastMovementDirection != Vector3.zero && currentState == MovementState.Grounded && Mathf.Approximately(GetCurrentSpeed(false), runSpeed);
            IsFlying = currentState == MovementState.Flying;
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
