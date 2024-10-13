using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JazzyLucas.Core
{
    public class Hoverable_ScaleChange : MonoBehaviour
    {
        [field: SerializeField] public Hoverable hoverable { get; private set; }
        [field: SerializeField] public Vector3 hoverScale { get; private set; } = new Vector3(1.1f, 1.1f, 1.1f);
        [field: SerializeField] public float animationDuration { get; private set; } = 0.2f;
        [field: Tooltip("Can be null.")]
        [field: SerializeField] public RectTransform targetRectTransform { get; private set; }
        [field: Tooltip("Can be null.")]
        [field: SerializeField] public Transform targetTransform { get; private set; }

        private Vector3 originalScale;
        private Coroutine coroutine;

        private void Awake()
        {
            if (targetTransform == null && targetRectTransform == null)
            {
                targetTransform = transform;
            }

            originalScale = targetTransform != null ? targetTransform.localScale : targetRectTransform.localScale;

            hoverable.OnHover += () =>
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                coroutine = StartCoroutine(AnimateScaleChange(originalScale, hoverScale));
            };

            hoverable.OnUnhover += () =>
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                coroutine = StartCoroutine(AnimateScaleChange(hoverScale, originalScale));
            };
        }

        private IEnumerator AnimateScaleChange(Vector3 startScale, Vector3 targetScale)
        {
            var timeElapsed = 0f;

            while (timeElapsed < animationDuration)
            {
                if (targetTransform != null)
                {
                    targetTransform.localScale = Vector3.Lerp(startScale, targetScale, timeElapsed / animationDuration);
                }
                else if (targetRectTransform != null)
                {
                    targetRectTransform.localScale = Vector3.Lerp(startScale, targetScale, timeElapsed / animationDuration);
                }

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            if (targetTransform != null)
            {
                targetTransform.localScale = targetScale;
            }
            else if (targetRectTransform != null)
            {
                targetRectTransform.localScale = targetScale;
            }
        }
    }
}
