using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JazzyLucas.Core
{
    /// <summary>
    /// Overlaps are like raycasts but should be persistent.
    /// <br></br>
    /// They can't collide with each other.
    /// </summary>
    public abstract class Overlap : MonoBehaviour
    {
        [field: SerializeField] public LayerMask LayerMask { get; private set; } = Physics.DefaultRaycastLayers;

        [field: SerializeField] public List<GameObject> GameObjectBlacklist { get; private set; } = new List<GameObject>();

        public event Action<Collider> OnOverlapEnter;
        public event Action<Collider> OnOverlapStay;
        public event Action<Collider> OnOverlapExit;

        private HashSet<Collider> currentColliders = new();

        public void DetectOverlap()
        {
            var colliders = PerformOverlap();

            HashSet<Collider> currentFrameColliders = new (colliders.Where(collider => !IsBlacklisted(collider)));

            // Enter
            foreach (var collider in currentFrameColliders.Where(collider => !currentColliders.Contains(collider)))
            {
                OnOverlapEnter?.Invoke(collider);
            }

            // Stay
            foreach (var collider in currentFrameColliders.Where(collider => currentColliders.Contains(collider)))
            {
                OnOverlapStay?.Invoke(collider);
            }

            // Exit
            foreach (var collider in currentColliders.Where(collider => !currentFrameColliders.Contains(collider)))
            {
                OnOverlapExit?.Invoke(collider);
            }

            currentColliders = currentFrameColliders;
        }

        private bool IsBlacklisted(Collider collider)
        {
            return GameObjectBlacklist.Contains(collider.gameObject);
        }

        protected abstract Collider[] PerformOverlap();

        protected virtual void OnDrawGizmos() { }
    }
}
