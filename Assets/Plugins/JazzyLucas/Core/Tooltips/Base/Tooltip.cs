using System;
using JazzyLucas.Core.Utils;
using UnityEngine;

namespace JazzyLucas.Core
{
    public abstract class Tooltip : MonoBehaviour
    {
        [field: SerializeField] public RectTransform Transform { get; private set; }
        [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }

        [field: SerializeField] public float scaleFactor { get; private set; } = 10f;
        [field: SerializeField] public float minScale { get; private set; } = 1f;
        [field: SerializeField] public float maxScale { get; private set; } = 10f;

        [field: HideInInspector] public Transform AnchorTo { get; set; }
        [field: HideInInspector] public Camera MainCamera { get; set; }

        private void Awake()
        {
            if (!Transform)
                Transform = transform as RectTransform;

            if (!MainCamera)
                MainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if (!AnchorTo)
            {
                Destroy(gameObject);
                return;
            }

            var viewportPoint = MainCamera.WorldToViewportPoint(AnchorTo.position);
            bool isVisible = viewportPoint is { z: > 0, x: > 0 and < 1, y: > 0 and < 1 };

            CanvasGroup.alpha = isVisible ? 1f : 0f;

            if (isVisible)
            {
                var distance = Vector3.Distance(MainCamera.transform.position, AnchorTo.position);
                var scale = Mathf.Clamp(scaleFactor / distance, minScale, maxScale);
                Transform.localScale = new Vector3(scale, scale, scale);
                UIUtil.AnchorToWorldSpacePoint(AnchorTo, Transform, MainCamera);
            }
        }
    }
}