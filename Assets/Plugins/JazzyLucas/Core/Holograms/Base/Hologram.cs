using System;
using JazzyLucas.Core.Utils;
using UnityEngine;

public abstract class Hologram : MonoBehaviour
{
    [field: HideInInspector] public RectTransform Transform { get; private set; }
    [field: HideInInspector] public Transform AnchorTo { get; set; }
    [field: HideInInspector] public Camera MainCamera { get; set; }

    [SerializeField] private float scaleFactor = 10f;
    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 10f;

    private void Awake()
    {
        if (!Transform)
        {
            Transform = (RectTransform)transform;
        }
    }

    private void LateUpdate()
    {
        if (!AnchorTo)
        {
            // TODO: destroy after a set amount of time? in case it's switching anchors?
            Destroy(this.gameObject);
            return;
        }

        // Calculate the distance between the camera and the anchor point
        float distance = Vector3.Distance(MainCamera.transform.position, AnchorTo.position);

        // Inverse the scale based on distance (closer = larger, farther = smaller)
        float scale = Mathf.Clamp(scaleFactor / distance, minScale, maxScale);

        // Apply the calculated scale to the RectTransform
        Transform.localScale = new Vector3(scale, scale, scale);

        // Position the UI element in world space
        UIUtil.AnchorToWorldSpacePoint(AnchorTo, Transform, MainCamera);
    }
}