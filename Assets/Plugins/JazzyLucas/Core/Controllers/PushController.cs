using System;
using UnityEngine;
using JazzyLucas.Core.Input;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    public class PushController : Controller
    {
        [field: SerializeField] public float PushForce { get; private set; } = 2f;

        [field: SerializeField] public CharacterControllerWrapper CCWrapper { get; private set; }
        private Transform Transform => CCWrapper.transform;

        public override void Init()
        {
            base.Init();
            CCWrapper.OnColliderHit += OnColliderColliderHit;
        }

        protected override void Process()
        {
            // nothing for now
        }

        private void OnColliderColliderHit(ControllerColliderHit hit)
        {
            var rb = hit.collider.attachedRigidbody;

            // Only push objects with a Rigidbody and not kinematic
            if (rb == null || rb.isKinematic) 
                return;

            // Check if the player is moving forward into the object
            Vector3 pushDirection = new(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.velocity = pushDirection * PushForce;
        }
    }
}
