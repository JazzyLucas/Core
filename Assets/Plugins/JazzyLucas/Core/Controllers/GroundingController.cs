using System;
using UnityEngine;
using JazzyLucas.Core.Input;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    public class GroundingController : Controller
    {
        [field: SerializeField] public ColliderWrapper ColliderWrapper { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }

        private Transform _groundedTransform; // The platform the player is grounded on
        private Vector3 _lastGroundedPosition; // Last frame's platform position
        
        private float _raycastDistance = 1.5f; // Adjust this distance as needed
        
        public override void Init()
        {
            base.Init();
            ColliderWrapper.OnTriggerEnterEvent += OnTriggerEnter;
            ColliderWrapper.OnTriggerStayEvent += OnTriggerStay;
            ColliderWrapper.OnTriggerExitEvent += OnTriggerExit;
        }

        protected override void Process()
        {
            UpdateGrounding();

            if (_groundedTransform != null && CharacterController.isGrounded)
            {
                // Calculate platform displacement (difference in position between frames)
                Vector3 platformMovement = _groundedTransform.position - _lastGroundedPosition;

                // Log platform movement and character movement for debugging
                L.Log("Platform Movement: " + platformMovement);
                L.Log("Character Position: " + CharacterController.transform.position);

                // Apply the platform's movement only if there's actual displacement
                if (platformMovement.sqrMagnitude > 0.0001f)
                {
                    CharacterController.Move(platformMovement);
                }

                // Update the last grounded position for the next frame
                _lastGroundedPosition = _groundedTransform.position;
            }
        }
        
        private void UpdateGrounding()
        {
            // Cast a ray downwards to detect what the character is standing on
            if (Physics.Raycast(CharacterController.transform.position, Vector3.down, out RaycastHit hit, _raycastDistance))
            {
                _groundedTransform = hit.transform;

                // Only update the platform position if the character is grounded
                if (CharacterController.isGrounded)
                {
                    _lastGroundedPosition = _groundedTransform.position;
                }
            }
            else
            {
                _groundedTransform = null; // Not grounded
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // You can still use the trigger system if necessary, but it's now secondary to raycasts
            UpdateGrounding();
        }

        private void OnTriggerStay(Collider other)
        {
            // Continuously update grounded transform while grounded
            UpdateGrounding();
        }

        private void OnTriggerExit(Collider other)
        {
            // Clear the grounded transform when leaving the platform
            _groundedTransform = null;
        }
    }
}
