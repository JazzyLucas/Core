using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzyLucas.Sample
{
    public class AtoBMover : MonoBehaviour
    {
        [field: SerializeField] public Transform movingTransform { get; private set; }
        [field: SerializeField] public Transform pointATransform { get; private set; }
        [field: HideInInspector] public Vector3 pointA => pointATransform.position;
        [field: SerializeField] public Transform pointBTransform { get; private set; }
        [field: HideInInspector] public Vector3 pointB => pointBTransform.position;
        [field: SerializeField] public float speed = 2.0f;
        [field: SerializeField] public float pauseDuration = 1.0f;

        private Vector3 targetPosition;
        private bool movingToB = true;
        private bool isPaused = false;

        private void Start()
        {
            movingTransform.position = pointA;
            targetPosition = pointB;
            StartCoroutine(MoveToTarget());
        }

        private IEnumerator MoveToTarget()
        {
            while (true)
            {
                // Move the platform
                while (Vector3.Distance(movingTransform.position, targetPosition) > 0.1f)
                {
                    movingTransform.position = Vector3.MoveTowards(movingTransform.position, targetPosition, speed * Time.deltaTime);
                    yield return null; // Wait until next frame
                }

                // Pause for the specified duration
                isPaused = true;
                yield return new WaitForSeconds(pauseDuration);
                isPaused = false;

                // Switch targets
                targetPosition = movingToB ? pointA : pointB;
                movingToB = !movingToB;
            }
        }
    }
}
