using System;
using JazzyLucas.Core.Utils;
using UnityEngine;

public abstract class Hologram : MonoBehaviour
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
            // TODO: destroy after a set amount of time? in case it's switching anchors?
            Destroy(gameObject);
            return;
        }

        // Check if the AnchorTo position is within the camera's view frustum
        var viewportPoint = MainCamera.WorldToViewportPoint(AnchorTo.position);
        bool isVisible = viewportPoint is { z: > 0, x: > 0 and < 1, y: > 0 and < 1 };

        // Set the alpha of the CanvasGroup based on visibility
        CanvasGroup.alpha = isVisible ? 1f : 0f;

        if (isVisible)
        {
            // Calculate the distance between the camera and the anchor point
            var distance = Vector3.Distance(MainCamera.transform.position, AnchorTo.position);

            // Inverse the scale based on distance (closer = larger, farther = smaller)
            var scale = Mathf.Clamp(scaleFactor / distance, minScale, maxScale);

            // Apply the calculated scale to the RectTransform
            Transform.localScale = new Vector3(scale, scale, scale);

            // Position the UI element in world space
            UIUtil.AnchorToWorldSpacePoint(AnchorTo, Transform, MainCamera);
        }
    }
}