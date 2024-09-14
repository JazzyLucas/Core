using UnityEngine;
using JazzyLucas.Core.Utils;

namespace JazzyLucas.Core
{
    public class BoxOverlap : Overlap
    {
        [field: SerializeField] public Vector3 BoxHalfExtents { get; private set; } = Vector3.one;

        protected override Collider[] PerformOverlap()
        {
            return PhysicsOverlapUtils.OverlapBox(transform.position, BoxHalfExtents, transform.rotation, LayerMask);
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, BoxHalfExtents * 2);
        }
    }
}