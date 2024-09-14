using UnityEngine;
using System.Collections.Generic;

namespace JazzyLucas.Core.Utils
{
    public static class PathDrawer
    {
        private static readonly Dictionary<Transform, LineRenderer> pathRenderers = new();
        private static Material defaultLineMaterial;

        public static void StartPath(Transform target, float width = 0.1f)
        {
            if (pathRenderers.ContainsKey(target))
                return;

            // Use the default material if none is provided
            if (defaultLineMaterial == null)
            {
                defaultLineMaterial = new Material(Shader.Find("Sprites/Default"));
                defaultLineMaterial.color = Color.white;
            }

            DoCreatePath(target, defaultLineMaterial, width);
        }
        public static void StartPath(Transform target, Material lineMaterial, float width = 0.1f)
        {
            if (pathRenderers.ContainsKey(target))
                return;

            DoCreatePath(target, lineMaterial, width);
        }

        public static void UpdatePath(Transform target, float pointSpacing = 0.1f)
        {
            if (!pathRenderers.TryGetValue(target, out var lineRenderer))
                return;

            // Check if it's the first point or if the target moved enough distance
            if (lineRenderer.positionCount == 0 ||
                Vector3.Distance(target.position, lineRenderer.GetPosition(lineRenderer.positionCount - 1)) >=
                pointSpacing)
            {
                // Add the new point
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, target.position);
            }
        }

        public static void ClearPath(Transform target)
        {
            if (!pathRenderers.TryGetValue(target, out var lineRenderer))
                return;

            lineRenderer.positionCount = 0; // Clear all points
        }

        public static void StopPath(Transform target)
        {
            if (!pathRenderers.TryGetValue(target, out var lineRenderer))
                return;

            Object.Destroy(lineRenderer.gameObject);
            pathRenderers.Remove(target);
        }

        private static void DoCreatePath(Transform target, Material lineMaterial, float width)
        {
            // Create a new GameObject to hold the LineRenderer
            var lineObject = new GameObject("PathDraw " + target.name);
            var lineRenderer = lineObject.AddComponent<LineRenderer>();

            // Set up LineRenderer properties
            lineRenderer.material = lineMaterial;
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
            lineRenderer.positionCount = 0;

            // Store the LineRenderer
            pathRenderers[target] = lineRenderer;
        }
    }
}