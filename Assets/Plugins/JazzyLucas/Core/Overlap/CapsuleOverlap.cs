using UnityEngine;
using JazzyLucas.Core.Utils;
using UnityEngine.Serialization;

namespace JazzyLucas.Core
{
    public class CapsuleOverlap : Overlap
    {
        [field: SerializeField] public Vector3 CapsulePoint0 { get; private set; } = Vector3.zero;
        [field: SerializeField] public Vector3 CapsulePoint1 { get; private set; } = Vector3.up;
        [field: SerializeField] public float CapsuleRadius { get; private set; } = 1f;

        protected override Collider[] PerformOverlap()
        {
            return PhysicsOverlapUtils.OverlapCapsule(transform.TransformPoint(CapsulePoint0), transform.TransformPoint(CapsulePoint1), CapsuleRadius, LayerMask);
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            DrawWireCapsule(transform.TransformPoint(CapsulePoint0), transform.TransformPoint(CapsulePoint1), CapsuleRadius);
        }

        private void DrawWireCapsule(Vector3 start, Vector3 end, float radius)
        {
            var up = (end - start).normalized;
            float height = Vector3.Distance(start, end);
            float sideHeight = Mathf.Max(0, height / 2 - radius);
            var side = Vector3.Cross(up, Vector3.right).normalized * radius;

            Gizmos.DrawWireSphere(start, radius);
            Gizmos.DrawWireSphere(end, radius);

            for (int i = 0; i < 36; i++)
            {
                float angle = i * 10 * Mathf.Deg2Rad;
                float nextAngle = (i + 1) * 10 * Mathf.Deg2Rad;

                var pointA = start + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, up) * side;
                var pointB = start + Quaternion.AngleAxis(nextAngle * Mathf.Rad2Deg, up) * side;
                var pointC = end + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, up) * side;
                var pointD = end + Quaternion.AngleAxis(nextAngle * Mathf.Rad2Deg, up) * side;

                Gizmos.DrawLine(pointA, pointB);
                Gizmos.DrawLine(pointC, pointD);
                Gizmos.DrawLine(pointA, pointC);
            }
        }
    }
}