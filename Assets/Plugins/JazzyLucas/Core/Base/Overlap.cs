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
            HashSet<Collider> newColliders = new(PerformOverlap().Where(collider => !IsBlacklisted(collider)));

            HandleEvents_WithCurrentColliders(newColliders);

            currentColliders = newColliders;
        }

        private void HandleEvents_WithCurrentColliders(HashSet<Collider> newColliders)
        {
            foreach (var collider in newColliders.Except(currentColliders))
            {
                OnOverlapEnter?.Invoke(collider);
            }

            foreach (var collider in newColliders.Intersect(currentColliders))
            {
                OnOverlapStay?.Invoke(collider);
            }

            foreach (var collider in currentColliders.Except(newColliders))
            {
                OnOverlapExit?.Invoke(collider);
            }
        }

        private bool IsBlacklisted(Collider collider) => GameObjectBlacklist.Contains(collider.gameObject);

        protected abstract Collider[] PerformOverlap();

        protected virtual void OnDrawGizmos() { }
    }
}
