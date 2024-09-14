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
        [field: SerializeField] public float overlapRadius = 0.5f;
        
        [field: Header("Debugging")]
        [field: SerializeField] public bool ShowDebugInOnGUI { get; private set; } = false;

        [field: HideInInspector] public bool _isTriggerEnterFired { get; private set; }
        [field: HideInInspector] public bool _isTriggerStayFired { get; private set; }
        [field: HideInInspector] public bool _isTriggerExitFired { get; private set; }
        
        private Transform _groundedTransform;
        private Vector3 _lastGroundedPosition = Vector3.zero;
        private Vector3 _platformMovement;
        
        public override void Init()
        {
            base.Init();
            ColliderWrapper.OnTriggerEnterEvent += OnTriggerEnter;
            ColliderWrapper.OnTriggerStayEvent += OnTriggerStay;
            ColliderWrapper.OnTriggerExitEvent += OnTriggerExit;
            
            // Ignore collision between the player and platform colliders
            Physics.IgnoreCollision(ColliderWrapper.GetComponent<Collider>(), CharacterController); // Example usage
        }

        protected override void Process()
        {
            // Check if ColliderWrapper is still colliding with something
            if (ColliderWrapper.CurrentTriggerCollider != null)
            {
                // Set _groundedTransform based on current collider if not set
                if (_groundedTransform == null)
                {
                    _groundedTransform = ColliderWrapper.CurrentTriggerCollider.transform;
                    _lastGroundedPosition = _groundedTransform.position;
                }

                // Calculate platform movement
                _platformMovement = _groundedTransform.position - _lastGroundedPosition;

                // Move the character with the platform
                CharacterController.Move(_platformMovement);

                // Update the last known position of the platform
                _lastGroundedPosition = _groundedTransform.position;

                // Update the path for the grounded transform
                PathDrawer.UpdatePath(_groundedTransform);
            }
            else
            {
                // Reset state if not grounded
                _groundedTransform = null;
                _lastGroundedPosition = Vector3.zero;
                _platformMovement = Vector3.zero;
            }
        }
        
        private void UpdateGrounding()
        {
            if (CharacterController.isGrounded && _groundedTransform != null)
            {
                _lastGroundedPosition = _groundedTransform.position;

                // Update the path for the grounded transform
                PathDrawer.UpdatePath(_groundedTransform);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            _isTriggerEnterFired = true;
            _isTriggerStayFired = false;
            _isTriggerExitFired = false;

            _groundedTransform = other.transform;

            // Start drawing the path for the grounded object
            PathDrawer.StartPath(_groundedTransform);

            UpdateGrounding();
        }

        private void OnTriggerStay(Collider other)
        {
            _isTriggerStayFired = true; // Set to true when the event fires

            if (_groundedTransform == null)
            {
                _groundedTransform = other.transform;

                // Start drawing the path for the grounded object
                PathDrawer.StartPath(_groundedTransform);

                UpdateGrounding();
            }

            if (_groundedTransform != null)
            {
                Debug.DrawLine(_lastGroundedPosition, _groundedTransform.position, Color.red);
                _lastGroundedPosition = _groundedTransform.position;

                // Update the path while staying grounded
                PathDrawer.UpdatePath(_groundedTransform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _isTriggerExitFired = true; // Set to true when the event fires
            _isTriggerStayFired = false; // Reset the stay flag

            if (_groundedTransform == other.transform)
            {
                // Stop drawing the path for the grounded object
                PathDrawer.StopPath(_groundedTransform);

                _groundedTransform = null;
                _lastGroundedPosition = Vector3.zero;
                _platformMovement = Vector3.zero;
            }
        }
    }
}
