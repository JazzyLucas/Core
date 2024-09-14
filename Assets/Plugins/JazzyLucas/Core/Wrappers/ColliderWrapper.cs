using System;
using UnityEngine;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    [RequireComponent(typeof(Collider))]
    public class ColliderWrapper : MonoBehaviour
    {
        [field: Header("Debugging")]
        [field: SerializeField] public bool LogCollisions { get; private set; } = false;
        
        #region Regular Collisions
        public Collision CurrentRegularCollision { get; private set; }
        public event Action<Collision> OnCollisionEnterEvent;
        public event Action<Collision> OnCollisionStayEvent;
        public event Action<Collision> OnCollisionExitEvent;

        private void OnCollisionEnter(Collision collision)
        {
            CurrentRegularCollision = collision;
            
            OnCollisionEnterEvent?.Invoke(collision);
            
            if (LogCollisions)
                L.Log($"OnCollisionEnter with {collision.gameObject.name}");
        }

        private void OnCollisionStay(Collision collision)
        {
            CurrentRegularCollision = collision;
            
            OnCollisionStayEvent?.Invoke(collision);
            
            if (LogCollisions)
                L.Log($"OnCollisionStay with {collision.gameObject.name}");
        }

        private void OnCollisionExit(Collision collision)
        {
            if (CurrentRegularCollision == collision)
                CurrentRegularCollision = null;
            
            OnCollisionExitEvent?.Invoke(collision);
            
            if (LogCollisions)
                L.Log($"OnCollisionExit with {collision.gameObject.name}");
        }
        #endregion

        #region Trigger Collisions
        public Collider CurrentTriggerCollider { get; private set; }
        public event Action<Collider> OnTriggerEnterEvent;
        public event Action<Collider> OnTriggerStayEvent;
        public event Action<Collider> OnTriggerExitEvent;

        private void OnTriggerEnter(Collider other)
        {
            CurrentTriggerCollider = other;
            
            OnTriggerEnterEvent?.Invoke(other);

            if (LogCollisions)
                L.Log($"OnTriggerEnter with {other.name}");
        }

        private void OnTriggerStay(Collider other)
        {
            CurrentTriggerCollider = other;
            
            OnTriggerStayEvent?.Invoke(other);
            
            if (LogCollisions)
                L.Log($"OnTriggerStay with {other.name}");
        }

        private void OnTriggerExit(Collider other)
        {
            if (CurrentTriggerCollider == other)
                CurrentTriggerCollider = null;
            
            OnTriggerExitEvent?.Invoke(other);

            if (LogCollisions)
                L.Log($"OnTriggerExit with {other.name}");
        }
        #endregion
    }
}
