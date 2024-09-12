using System;
using UnityEngine;
using JazzyLucas.Core.Input;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    public class PushController : Controller
    {
        [field: SerializeField] public CharacterControllerWrapper CCWrapper { get; private set; }
        [field: SerializeField] public float PushForce { get; private set; } = 0.05f;

        public override void Init()
        {
            base.Init();
            CCWrapper.OnColliderHitEvent += OnColliderHitEvent;
        }

        protected override void Process()
        {
            // nothing for now
        }

        private void OnColliderHitEvent(ControllerColliderHit hit)
        {
            var rb = hit.collider.attachedRigidbody;

            if (rb == null || rb.isKinematic)
                return;

            var pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z).normalized;

            rb.AddForce(pushDirection * PushForce, ForceMode.VelocityChange);
        }
    }
}