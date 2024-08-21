using System;
using JazzyLucas.Core.Utils;
using UnityEngine;

namespace JazzyLucas.Core
{
    public abstract class Tooltip : MonoBehaviour
    {
        [field: SerializeField] public RectTransform Transform { get; private set; }

        private void Awake()
        {
            if (!Transform)
                Transform = transform as RectTransform;
        }

        private void LateUpdate()
        {
            Transform.position = UnityEngine.Input.mousePosition;
        }
    }
}