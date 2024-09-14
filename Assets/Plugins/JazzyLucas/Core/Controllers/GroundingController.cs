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
        
        [field: Header("Debugging")]
        [field: SerializeField] public bool ShowDebugInOnGUI { get; private set; } = false;

        [field: HideInInspector] public bool _isTriggerEnterFired { get; private set; }
        [field: HideInInspector] public bool _isTriggerStayFired { get; private set; }
        [field: HideInInspector] public bool _isTriggerExitFired { get; private set; }
        
        private Transform _groundedTransform; // The platform the player is grounded on
        private Vector3 _lastGroundedPosition; // Last frame's platform position
        
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
            // nothing for now
        }
        
        private void OnGUI()
        {
            if (!ShowDebugInOnGUI)
                return;
    
            GUIStyle labelStyle = new(GUI.skin.label)
            {
                fontSize = 20,
                normal = new() { textColor = Color.white }
            };

            // Display collision event flags
            GUI.Label(new(10, 160, 300, 25), $"TriggerEnter Fired: {_isTriggerEnterFired}", labelStyle);
            GUI.Label(new(10, 190, 300, 25), $"TriggerStay Fired: {_isTriggerStayFired}", labelStyle);
            GUI.Label(new(10, 220, 300, 25), $"TriggerExit Fired: {_isTriggerExitFired}", labelStyle);
        }
        
        private void UpdateGrounding()
        {
            if (CharacterController.isGrounded && _groundedTransform != null)
            {
                _lastGroundedPosition = _groundedTransform.position;

                // Update the path with the current grounded object
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

                UpdateGrounding();
            }

            if (_groundedTransform != null)
            {
                Debug.DrawLine(_lastGroundedPosition, _groundedTransform.position, Color.red);
                _lastGroundedPosition = _groundedTransform.position;

                PathDrawer.UpdatePath(_groundedTransform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _isTriggerExitFired = true; // Set to true when the event fires
            _isTriggerStayFired = false; // Reset the stay flag

            if (_groundedTransform == other.transform)
            {
                PathDrawer.StopPath(_groundedTransform);

                _groundedTransform = null;
                _lastGroundedPosition = Vector3.zero;
            }
        }
    }
}
