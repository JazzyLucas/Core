using System;
using UnityEngine;
using JazzyLucas.Core.Input;
using JazzyLucas.Core.Utils;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    public class GroundingController : Controller
    {
        [field: SerializeField] public ColliderWrapper ColliderWrapper { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public float overlapRadius = 0.5f; // Radius for the overlap sphere
        
        [field: Header("Debugging")]
        [field: SerializeField] public bool ShowDebugInOnGUI { get; private set; } = false;

        [field: HideInInspector] public bool IsTriggerEnterFired { get; private set; }
        [field: HideInInspector] public bool IsCurrentlyColliding => ColliderWrapper.CurrentTriggerCollider != null;
        [field: HideInInspector] public bool IsTriggerExitFired { get; private set; }
        
        [field: HideInInspector] public Transform CollisionStartPoint { get; private set; }
        [field: HideInInspector] public Transform CollisionCurrentPoint => ColliderWrapper.CurrentTriggerCollider.transform;
        [field: HideInInspector] public Transform CollisionEndPoint { get; private set; }
        
        [field: HideInInspector] public Vector3 LatestProcessCollisionPosition { get; private set; } = Vector3.zero;
        [field: HideInInspector] public Vector3 PreviousProcessCollisionPosition { get; private set; } = Vector3.zero;

        [field: HideInInspector] public Vector3 DistanceFromLastProcess => LatestProcessCollisionPosition - PreviousProcessCollisionPosition;
        
        public override void Init()
        {
            base.Init();
            ColliderWrapper.OnTriggerEnterEvent += OnTriggerEnter;
            ColliderWrapper.OnTriggerExitEvent += OnTriggerExit;
            
            // Ignore collision between the player and platform colliders
            Physics.IgnoreCollision(ColliderWrapper.GetComponent<Collider>(), CharacterController);
        }

        protected override void Process()
        {
            if (IsCurrentlyColliding)
            {
                if (CollisionCurrentPoint == null && CollisionStartPoint == null)
                {
                    L.Log("Currently colliding with something without a StartPoint / Enter event? (race condition?). Bailing.");
                    return;
                }
                
                //CharacterController.Move(DistanceFromLastProcess);

                PreviousProcessCollisionPosition = LatestProcessCollisionPosition;
                LatestProcessCollisionPosition = CollisionCurrentPoint.gameObject.transform.position;
                    
                PathDrawer.UpdatePath(CollisionCurrentPoint);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            IsTriggerEnterFired = true;
            IsTriggerExitFired = false;

            CollisionStartPoint = other.transform;
            
            PathDrawer.StartPath(CollisionStartPoint);
        }

        private void OnTriggerExit(Collider other)
        {
            IsTriggerEnterFired = false;
            IsTriggerExitFired = true;

            CollisionEndPoint = other.transform;
            
            PathDrawer.ClearPath(CollisionStartPoint);
            
            ResetCollision();
        }

        private void ResetCollision()
        {
            CollisionStartPoint = null;
        }
    }
}
