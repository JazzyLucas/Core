using UnityEngine;
using JazzyLucas.Core.Utils;

namespace JazzyLucas.Core
{
    public class SphereOverlap : Overlap
    {
        [field: SerializeField] public float SphereRadius { get; private set; } = 1f;

        protected override Collider[] PerformOverlap()
        {
            return PhysicsOverlapUtils.OverlapSphere(transform.position, SphereRadius, LayerMask);
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, SphereRadius);
        }
    }
}