using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzyLucas.Core
{
    public static class VisualUtils
    {
        public static List<Component> CollectVisuals(Transform parent)
        {
            List<Component> visuals = new();
            CollectVisualsRecursively(parent, visuals);
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
    }
}
