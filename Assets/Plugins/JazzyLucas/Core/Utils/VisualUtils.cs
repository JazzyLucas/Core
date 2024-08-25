using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzyLucas.Core
{
    public static class VisualUtils
    {
        public static List<Component> CollectVisuals(Transform root)
        {
            List<Component> visuals = new();
            CollectVisualsRecursively(root, visuals);
            return visuals;
        }
        private static void CollectVisualsRecursively(Transform parent, List<Component> visuals)
        {
            visuals.AddRange(parent.GetComponents<MeshRenderer>());
            visuals.AddRange(parent.GetComponents<SpriteRenderer>());
            visuals.AddRange(parent.GetComponents<SkinnedMeshRenderer>());
            visuals.AddRange(parent.GetComponents<CanvasRenderer>());
            visuals.AddRange(parent.GetComponents<ParticleSystemRenderer>());
            foreach (Transform child in parent)
                CollectVisualsRecursively(child, visuals);
        }
        
        public static void ToggleVisuals(Transform rootOfVisuals, bool state)
        {
            var visuals = CollectVisuals(rootOfVisuals);
            ToggleVisuals(visuals, state);
        }
        public static void ToggleVisuals(List<Component> visuals, bool state)
        {
            foreach (var component in visuals)
            {
                if (component is Renderer renderer)
                {
                    renderer.enabled = state;
                }
                else if (component is CanvasRenderer canvasRenderer)
                {
                    canvasRenderer.SetAlpha(state ? 1f : 0f);
                }
                else if (component is ParticleSystemRenderer particleRenderer)
                {
                    particleRenderer.enabled = state;
                }
            }
        }
    }
}
