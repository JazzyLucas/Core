using System;
using System.Collections.Generic;
using UnityEngine;
using JazzyLucas.Core.Input;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core.Utils
{
    public static class PhysicsOverlapUtils
    {
        public static Collider[] OverlapBox(Vector3 center, Vector3 halfExtents, Quaternion orientation, int layerMask = Physics.DefaultRaycastLayers, bool logResult = false)
        {
            var colliders = Physics.OverlapBox(center, halfExtents, orientation, layerMask);
            if (logResult)
                LogOverlap("OverlapBox", colliders);
            return colliders;
        }

        public static Collider[] OverlapCapsule(Vector3 point0, Vector3 point1, float radius, int layerMask = Physics.DefaultRaycastLayers, bool logResult = false)
        {
            var colliders = Physics.OverlapCapsule(point0, point1, radius, layerMask);
            if (logResult)
                LogOverlap("OverlapCapsule", colliders);
            return colliders;
        }

        public static Collider[] OverlapSphere(Vector3 position, float radius, int layerMask = Physics.DefaultRaycastLayers, bool logResult = false)
        {
            var colliders = Physics.OverlapSphere(position, radius, layerMask);
            if (logResult)
                LogOverlap("OverlapSphere", colliders);
            return colliders;
        }

        private static void LogOverlap(string methodName, Collider[] colliders)
        {
            if (colliders == null || colliders.Length == 0)
            {
                L.Log($"{methodName}: No colliders found.");
                return;
            }

            var colliderNames = new List<string>();
            foreach (var collider in colliders)
            {
                colliderNames.Add(collider.name);
            }

            L.Log($"{methodName}: Overlapped with {colliders.Length} colliders: {string.Join(", ", colliderNames)}");
        }
    }
}
