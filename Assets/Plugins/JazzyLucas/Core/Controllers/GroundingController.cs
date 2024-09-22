using System;
using UnityEngine;
using JazzyLucas.Core.Utils;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    public class GroundingController : Controller
    {
        [field: SerializeField] public Overlap Overlap { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        
        [field: SerializeField] public MovementController MovementController { get; private set; }
        public bool MovCont_IsInAir => MovementController.IsInAir || MovementController.IsJumping;

        [field: Header("Debugging")]
        [field: Tooltip("Draw a LineRenderer Path as it follows the transform of a grounded-to Transform.")]
        [field: SerializeField] public bool Debug_DrawPath { get; private set; } = false;
        [field: SerializeField] public bool ShowDebugInOnGUI { get; private set; } = false;

        // Readable state fields for things like animators
        [field: HideInInspector] public bool IsGroundedOnPlatform { get; private set; }
        [field: HideInInspector] public bool IsMovingOnPlatform { get; private set; }
        [field: HideInInspector] public bool IsStandingStillOnPlatform { get; private set; }
        
        [field: HideInInspector] public bool IsCurrentlyColliding => currentCollider != null;
        [field: HideInInspector] public Vector3 Latest_Collider_Position { get; private set; }
        [field: HideInInspector] public Vector3 Previous_Collider_Position { get; private set; }
        [field: HideInInspector] public Vector3 DistanceFromLastProcess => Latest_Collider_Position - Previous_Collider_Position;

        private Collider currentCollider;
        private bool isFirstFrameAfterGrounding = false;

        public override void Init()
        {
            base.Init();
            
            foreach (var collider in CharacterController.GetComponentsInChildren<Collider>(true))
            {
                Overlap.GameObjectBlacklist.Add(collider.gameObject);
            }
            
            Overlap.OnOverlapEnter += OnOverlapEnter;
            Overlap.OnOverlapStay += OnOverlapStay;
            Overlap.OnOverlapExit += OnOverlapExit;
        }
        
        private void OnGUI()
        {
            if (!ShowDebugInOnGUI)
                return;

            GUIStyle labelStyle = new(GUI.skin.label)
            {
                fontSize = 20,
                normal = new() { textColor = Color.cyan }
            };
    
            const int yOffset = 180;
            GUI.Label(new(10, yOffset, 300, 25), $"IsGroundedOnPlatform: {IsGroundedOnPlatform}", labelStyle);
            GUI.Label(new(10, yOffset + 30, 300, 25), $"IsMovingOnPlatform: {IsMovingOnPlatform}", labelStyle);
            GUI.Label(new(10, yOffset + 60, 300, 25), $"IsStandingStillOnPlatform: {IsStandingStillOnPlatform}", labelStyle);
            GUI.Label(new(10, yOffset + 90, 300, 25), $"IsCurrentlyColliding: {IsCurrentlyColliding}", labelStyle);
        }

        protected override void Process()
        {
            Overlap.DetectOverlap();
            UpdateStateBooleans();
        }

        private void OnOverlapEnter(Collider other)
        {
            if (currentCollider == null)
            {
                currentCollider = other;
                InitializeColliderPositions();
                isFirstFrameAfterGrounding = true;
                if (Debug_DrawPath)
                    PathDrawer.StartPath(other.transform);
            }
        }

        private void OnOverlapStay(Collider other)
        {
            if (currentCollider == other && !MovCont_IsInAir)
            {
                if (isFirstFrameAfterGrounding)
                {
                    Previous_Collider_Position = currentCollider.transform.position;
                    Latest_Collider_Position = currentCollider.transform.position;
            
                    isFirstFrameAfterGrounding = false;
                    return;
                }

                var platformMovement = new Vector3(DistanceFromLastProcess.x, 0f, DistanceFromLastProcess.z);

                if (platformMovement.sqrMagnitude > Mathf.Epsilon)
                {
                    CharacterController.Move(platformMovement);
                    Debug.Log(platformMovement);
                }

                Previous_Collider_Position = Latest_Collider_Position;
                Latest_Collider_Position = currentCollider.transform.position;

                if (Debug_DrawPath)
                    PathDrawer.UpdatePath(currentCollider.transform);
            }
        }


        private void OnOverlapExit(Collider other)
        {
            if (currentCollider == other)
            {
                if (Debug_DrawPath)
                    PathDrawer.ClearPath(other.transform);
                ResetCollision();
            }
        }

        private void OnDestroy()
        {
            Overlap.OnOverlapEnter -= OnOverlapEnter;
            Overlap.OnOverlapStay -= OnOverlapStay;
            Overlap.OnOverlapExit -= OnOverlapExit;
        }

        private void InitializeColliderPositions()
        {
            if (currentCollider != null)
            {
                Latest_Collider_Position = currentCollider.transform.position;
                Previous_Collider_Position = currentCollider.transform.position;
            }
        }

        private void ResetCollision()
        {
            currentCollider = null;
        }

        private void UpdateStateBooleans()
        {
            IsGroundedOnPlatform = currentCollider != null && !MovCont_IsInAir;
            IsMovingOnPlatform = IsGroundedOnPlatform && DistanceFromLastProcess.sqrMagnitude > Mathf.Epsilon;
            IsStandingStillOnPlatform = IsGroundedOnPlatform && DistanceFromLastProcess.sqrMagnitude <= Mathf.Epsilon;
        }
    }
}
