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
        [field: SerializeField] public float OverlapRadius { get; private set; } = 0.5f; // Radius for the overlap sphere

        [field: Header("Debugging")]
        [field: SerializeField] public bool ShowDebugInOnGUI { get; private set; } = false;

        // Overlap tracking
        private Collider currentCollider;
        
        [field: HideInInspector] public bool IsCurrentlyColliding => currentCollider != null;

        [field: HideInInspector] public Transform CollisionStartPoint { get; private set; }
        [field: HideInInspector] public Transform CollisionCurrentPoint => currentCollider?.transform;
        [field: HideInInspector] public Transform CollisionEndPoint { get; private set; }

        [field: HideInInspector] public Vector3 LatestProcessCollisionPosition { get; private set; } = Vector3.zero;
        [field: HideInInspector] public Vector3 PreviousProcessCollisionPosition { get; private set; } = Vector3.zero;

        [field: HideInInspector] public Vector3 DistanceFromLastProcess => LatestProcessCollisionPosition - PreviousProcessCollisionPosition;

        public override void Init()
        {
            base.Init();
            
            Overlap.OnOverlapEnter += OnOverlapEnter;
            Overlap.OnOverlapStay += OnOverlapStay;
            Overlap.OnOverlapExit += OnOverlapExit;
        }

        protected override void Process()
        {
            Overlap.DetectOverlap();

            if (IsCurrentlyColliding)
            {
                if (CollisionCurrentPoint == null && CollisionStartPoint == null)
                {
                    L.Log("Currently colliding with something without a StartPoint / Enter event? (race condition?). Bailing.");
                    return;
                }

                // Update the position for movement calculation
                PreviousProcessCollisionPosition = LatestProcessCollisionPosition;
                LatestProcessCollisionPosition = CollisionCurrentPoint.position;

                // Calculate the distance to move in X and Z, ignoring Y
                var distanceToMove = new Vector3(DistanceFromLastProcess.x, 0f, DistanceFromLastProcess.z);

                // Move the character controller only in X/Z
                CharacterController.Move(distanceToMove);

                // Draw the path for debugging
                PathDrawer.UpdatePath(CollisionCurrentPoint);
            }
        }


        private void OnOverlapEnter(Collider other)
        {
            // This is called when a collider enters the overlap area
            if (currentCollider == null)
            {
                currentCollider = other;
                CollisionStartPoint = other.transform;
                
                PathDrawer.StartPath(CollisionStartPoint);
            }
        }

        private void OnOverlapStay(Collider other)
        {
            // This is called when a collider stays within the overlap area
            // Optional: Implement logic if needed to handle continuous stay
        }

        private void OnOverlapExit(Collider other)
        {
            // This is called when a collider exits the overlap area
            if (currentCollider == other)
            {
                CollisionEndPoint = other.transform;
                
                PathDrawer.ClearPath(CollisionStartPoint);
                
                ResetCollision();
            }
        }

        private void ResetCollision()
        {
            CollisionStartPoint = null;
            currentCollider = null;
        }

        private void OnDestroy()
        {
            Overlap.OnOverlapEnter -= OnOverlapEnter;
            Overlap.OnOverlapStay -= OnOverlapStay;
            Overlap.OnOverlapExit -= OnOverlapExit;
        }
    }
}
