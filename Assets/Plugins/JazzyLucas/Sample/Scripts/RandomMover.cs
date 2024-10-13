using UnityEngine;

namespace JazzyLucas.Sample
{
    public class RandomMover : MonoBehaviour
    {
        public float moveSpeed = 2f;
        public float minDistance = 0.1f;
        public float maxDistance = 5f; // Maximum distance from the initial position
        public float jitterAmount = 0f; // Set this to a value greater than 0 for jitter

        private Vector3 initialPosition; // The starting position of the object
        private Vector3 targetPosition;

        private void Start()
        {
            // Store the initial position of the object
            initialPosition = transform.position;
            SetRandomDestination();
        }

        private void Update()
        {
            MoveTowardsTarget();
        }

        private void SetRandomDestination()
        {
            // Set a random destination within the maxDistance from the initial position
            targetPosition = new Vector3(
                Random.Range(initialPosition.x - maxDistance, initialPosition.x + maxDistance),
                initialPosition.y,
                Random.Range(initialPosition.z - maxDistance, initialPosition.z + maxDistance)
            );
        }

        private void MoveTowardsTarget()
        {
            // Apply jitter if jitterAmount is greater than 0
            var jitter = Vector3.zero;
            if (jitterAmount > 0f)
            {
                jitter = new Vector3(
                    Random.Range(-jitterAmount, jitterAmount),
                    0f,
                    Random.Range(-jitterAmount, jitterAmount)
                );
            }

            // Move towards the target position with jitter
            transform.position =
                Vector3.MoveTowards(transform.position, targetPosition + jitter, moveSpeed * Time.deltaTime);

            // Check if the object is "close enough" to the target position, considering jitter
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget < minDistance + jitterAmount)
            {
                SetRandomDestination();
            }
        }
    }
}