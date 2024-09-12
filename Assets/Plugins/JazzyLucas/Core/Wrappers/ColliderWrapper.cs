using System;
using UnityEngine;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    [RequireComponent(typeof(Collider))]
    public class ColliderWrapper : MonoBehaviour
    {
        #region Regular Collisions
        public event Action<Collision> OnCollisionEnterEvent;
        public event Action<Collision> OnCollisionStayEvent;
        public event Action<Collision> OnCollisionExitEvent;

        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionEnterEvent?.Invoke(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            OnCollisionStayEvent?.Invoke(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            OnCollisionExitEvent?.Invoke(collision);
        }
        #endregion

        #region Trigger Collisions
        public event Action<Collider> OnTriggerEnterEvent;
        public event Action<Collider> OnTriggerStayEvent;
        public event Action<Collider> OnTriggerExitEvent;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayEvent?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitEvent?.Invoke(other);
        }
        #endregion
    }
}