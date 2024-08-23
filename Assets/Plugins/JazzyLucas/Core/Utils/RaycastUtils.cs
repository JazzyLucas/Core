using UnityEngine;

namespace JazzyLucas.Core.Utils
{
    public static class RaycastUtils
    {
        public static bool RaycastHitObject(out GameObject go, Transform transform, Color color = default)
        {
            return RaycastHitObject(out go, transform, ~0, color); // ~0 is the default LayerMask that interacts with all layers
        }

        public static bool RaycastHitObject(out GameObject go, Transform transform, LayerMask layerMask, Color color = default)
        {
            go = null;

            var start = transform.position;
            var direction = transform.forward;

            Debug.DrawLine(start, start + direction * 100, color == default ? Color.green : color); // TODO: adjustable raycast distance
            if (Physics.Raycast(start, direction, out var hit, 100, layerMask))
            {
                go = hit.collider.gameObject;
            }
            
            return go != null;
        }
    }
}