using UnityEngine;

public class RandomMover : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float minDistance = 0.1f;
    public float jitterAmount = 0f; // Set this to a value greater than 0 for jitter

    private Vector3 targetPosition;

    private void Start()
    {
        SetRandomDestination();
    }

    private void Update()
    {
        MoveTowardsTarget();
    }

    private void SetRandomDestination()
    {
        targetPosition = new(
            Random.Range(0f, 10f),
            transform.position.y,
            Random.Range(0f, 10f)
        );
    }

    private void MoveTowardsTarget()
    {
        // Apply jitter if jitterAmount is greater than 0
        var jitter = Vector3.zero;
        if (jitterAmount > 0f)
        {
            jitter = new(
                Random.Range(-jitterAmount, jitterAmount),
                0f,
                Random.Range(-jitterAmount, jitterAmount)
            );
        }

        // Move towards the target position with jitter
        transform.position = Vector3.MoveTowards(transform.position, targetPosition + jitter, moveSpeed * Time.deltaTime);

        // Check if the object is "close enough" to the target position, considering jitter
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if (distanceToTarget < minDistance + jitterAmount)
        {
            SetRandomDestination();
        }
    }
}