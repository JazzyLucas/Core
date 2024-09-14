using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JazzyLucas.Core
{
    public abstract class Overlap : MonoBehaviour
    {
        public event Action<Collider> OnOverlapEnter;
        public event Action<Collider> OnOverlapStay;
        public event Action<Collider> OnOverlapExit;

        [field: SerializeField] public LayerMask LayerMask { get; private set; } = Physics.DefaultRaycastLayers;

        // Track current overlapping colliders
        private HashSet<Collider> currentColliders = new HashSet<Collider>();

        public void DetectOverlap()
        {
            var colliders = PerformOverlap();

            HashSet<Collider> currentFrameColliders = new(colliders);

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

        protected abstract Collider[] PerformOverlap();

        protected virtual void OnDrawGizmos() { }
    }
}