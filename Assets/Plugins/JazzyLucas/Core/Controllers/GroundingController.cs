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

        private Collider currentCollider;

        [field: HideInInspector] public bool IsCurrentlyColliding => currentCollider != null;

        [field: HideInInspector] public Vector3 Latest_Collider_Position { get; private set; }
        [field: HideInInspector] public Vector3 Previous_Collider_Position { get; private set; }
        [field: HideInInspector] public Vector3 DistanceFromLastProcess => Latest_Collider_Position - Previous_Collider_Position;

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

        protected override void Process()
        {
            Overlap.DetectOverlap();
        }

        private void OnOverlapEnter(Collider other)
        {
            if (currentCollider == null)
            {
                currentCollider = other;
                InitializeColliderPositions();
                isFirstFrameAfterGrounding = true;
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

                PathDrawer.UpdatePath(currentCollider.transform);
            }
        }


        private void OnOverlapExit(Collider other)
        {
            if (currentCollider == other)
            {
                PathDrawer.ClearPath(other.transform);
                ResetCollision();
            }
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

        private void OnDestroy()
        {
            Overlap.OnOverlapEnter -= OnOverlapEnter;
            Overlap.OnOverlapStay -= OnOverlapStay;
            Overlap.OnOverlapExit -= OnOverlapExit;
        }
    }
}
