using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMover : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float minDistance = 0.1f;

    private Vector3 targetPosition;

    void Start()
    {
        SetRandomDestination();
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void SetRandomDestination()
    {
        var randomX = Random.Range(0f, 10f);
        var randomZ = Random.Range(0f, 10f);
        targetPosition = new(randomX, transform.position.y, randomZ);
    }

    void MoveTowardsTarget()
    {
        // Move the object towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the object is close enough to the target position
        if (Vector3.Distance(transform.position, targetPosition) < minDistance)
        {
            SetRandomDestination();
        }
    }
}
