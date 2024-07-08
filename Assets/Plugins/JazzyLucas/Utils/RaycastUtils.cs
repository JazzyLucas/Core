using UnityEngine;

namespace JazzyLucas.Utils
{
    public static class RaycastUtils
    {
        public static bool RaycastHitObject(out GameObject go, Transform transform, Color color = default)
        {
            go = null;

            var start = transform.position;
            var direction = transform.forward;

            Debug.DrawLine(start, start + direction * 100, color == default ? Color.green : color);
            if (Physics.Raycast(start, direction, out var hit, 100))
            {
                go = hit.collider.gameObject;
            }
            
            return go != null;
        }
    }
}