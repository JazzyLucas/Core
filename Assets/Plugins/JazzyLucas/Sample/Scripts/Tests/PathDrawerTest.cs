using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core.Utils;
using UnityEngine;

namespace JazzyLucas.Sample
{
    public class PathDrawerTest : MonoBehaviour
    {
        public Transform target;
        public Material lineMaterial;
        public float pathWidth = 0.1f;
        public float pointSpacing = 0.1f;

        private void Start()
        {
            PathDrawer.StartPath(target, lineMaterial, pathWidth);
        }

        private void Update()
        {
            PathDrawer.UpdatePath(target, pointSpacing);
        }
    }
}
