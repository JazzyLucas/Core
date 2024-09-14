using System;
using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core.Utils;
using UnityEngine;

namespace JazzyLucas.Core
{
    public class OverlapController : Controller
    {
        [field: SerializeField] public OverlapType OverlapType { get; private set; } = OverlapType.Box;
        [field: SerializeField] public Vector3 BoxHalfExtents { get; private set; } = Vector3.one;
        [field: SerializeField] public float SphereRadius { get; private set; } = 1f;
        [field: SerializeField] public Vector3 CapsulePoint0 { get; private set; } = Vector3.zero;
        [field: SerializeField] public Vector3 CapsulePoint1 { get; private set; } = Vector3.up;
        [field: SerializeField] public float CapsuleRadius { get; private set; } = 1f;
        [field: SerializeField] public LayerMask LayerMask { get; private set; } = Physics.DefaultRaycastLayers;

        public event Action<Collider[]> OnOverlapDetected;

        protected override void Process()
        {
            var detectedColliders = OverlapType switch
            {
                OverlapType.Box => PhysicsOverlapUtils.OverlapBox(transform.position, BoxHalfExtents,
                    transform.rotation, LayerMask),
                OverlapType.Capsule => PhysicsOverlapUtils.OverlapCapsule(transform.TransformPoint(CapsulePoint0),
                    transform.TransformPoint(CapsulePoint1), CapsuleRadius, LayerMask),
                OverlapType.Sphere => PhysicsOverlapUtils.OverlapSphere(transform.position, SphereRadius, LayerMask),
                _ => null
            };

            OnOverlapDetected?.Invoke(detectedColliders);
        }
    }
    
    public enum OverlapType
    {
        Box,
        Capsule,
        Sphere
    }
}
