using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JazzyLucas.Core.Utils
{
    public static class TransformUtil
    {
        public static void DestroyAllChildren(Transform parent)
        {
            var children = parent.Cast<Transform>().Select(child => child.gameObject).ToList();
            foreach (var child in children) 
                Object.Destroy(child);
        }
    }
}