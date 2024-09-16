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

        // Overlap tracking
        private Collider currentCollider;
        
        [field: HideInInspector] public bool IsCurrentlyColliding => currentCollider != null;

        [field: HideInInspector] public Transform CollisionStartPoint { get; private set; }
        [field: HideInInspector] public Transform CollisionCurrentPoint => currentCollider?.transform;
        [field: HideInInspector] public Transform CollisionEndPoint { get; private set; }

        [field: HideInInspector] public Vector3 Latest_Collider_Position { get; private set; }
        [field: HideInInspector] public Vector3 Previous_Collider_Position { get; private set; }

        [field: HideInInspector] public Vector3 DistanceFromLastProcess => Latest_Collider_Position - Previous_Collider_Position;

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

        protected override void Process()
        {
            Overlap.DetectOverlap();

            if (IsCurrentlyColliding && !MovCont_IsInAir)
            {
                if (CollisionCurrentPoint == null && CollisionStartPoint == null)
                {
                    L.Log("Currently colliding with something without a StartPoint / Enter event? (race condition?). Bailing.");
                    return;
                }

                var platformMovement = new Vector3(DistanceFromLastProcess.x, 0f, DistanceFromLastProcess.z);
                
                // Apply movement only if the platformMovement vector is valid
                if (platformMovement.sqrMagnitude > Mathf.Epsilon) 
                {
                    CharacterController.Move(platformMovement);
                }

                Previous_Collider_Position = Latest_Collider_Position;
                Latest_Collider_Position = CollisionCurrentPoint.position;

                PathDrawer.UpdatePath(CollisionCurrentPoint);

                L.Log($"{currentCollider.gameObject.name}");
            }
        }

        private void OnOverlapEnter(Collider other)
        {
            if (currentCollider == null)
            {
                currentCollider = other;
                CollisionStartPoint = other.transform;

                // Initialize the collider positions to avoid large platform movement on the first process
                Latest_Collider_Position = CollisionCurrentPoint.position;
                Previous_Collider_Position = CollisionCurrentPoint.position;

                PathDrawer.StartPath(CollisionStartPoint);
            }
        }

        private void OnOverlapStay(Collider other)
        {
            // nothing yet
        }

        private void OnOverlapExit(Collider other)
        {
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
