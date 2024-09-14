using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDrawerTest : MonoBehaviour
{
    public Transform target; // Object to track
    public Material lineMaterial; // Material for the line
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

    private void OnDestroy()
    {
        PathDrawer.StopPath(target);
    }
}
