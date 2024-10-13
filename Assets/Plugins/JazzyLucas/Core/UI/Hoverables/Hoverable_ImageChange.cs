using System.Collections;
using JazzyLucas.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace JazzyLucas.Core
{
    public class Hoverable_ImageChange : MonoBehaviour
    {
        [field: SerializeField] public Hoverable hoverable { get; private set; }
        [field: Tooltip("Can be null.")]
        [field: SerializeField] public Graphic originalGraphic { get; private set; }
        [field: Tooltip("Can be null.")]
        [field: SerializeField] public Graphic hoverGraphic { get; private set; }
        [field: SerializeField] public float animationDuration { get; private set; } = 0.1f;
        [field: SerializeField] public bool showOriginalGraphic { get; private set; } = false;

        private float originalAlpha;
        private Coroutine coroutine;

        private void Awake()
        {
            // Handle null checks for hoverGraphic and originalGraphic
            if (hoverGraphic != null)
            {
                originalAlpha = hoverGraphic.color.a;
                UIUtil.SetGraphicAlpha(hoverGraphic, 0f);
            }

            hoverable.OnHover += () =>
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                if (hoverGraphic != null)
                {
                    hoverGraphic.gameObject.SetActive(true);
                }

                coroutine = StartCoroutine(AnimateGraphicChange(showOriginalGraphic ? null : originalGraphic, hoverGraphic));
            };

            hoverable.OnUnhover += () =>
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                if (originalGraphic != null)
                {
                    originalGraphic.gameObject.SetActive(true);
                }

                coroutine = StartCoroutine(AnimateGraphicChange(hoverGraphic, showOriginalGraphic ? null : originalGraphic));
            };
        }

        private IEnumerator AnimateGraphicChange(Graphic fadeOutGraphic, Graphic fadeInGraphic)
        {
            var timeElapsed = 0f;
            var fadeOutStartAlpha = fadeOutGraphic != null ? fadeOutGraphic.color.a : 0f;
            var fadeInTargetAlpha = fadeInGraphic != null ? originalAlpha : 0f;

            while (timeElapsed < animationDuration)
            {
                var alpha = timeElapsed / animationDuration;

                if (fadeOutGraphic != null)
                {
                    UIUtil.SetGraphicAlpha(fadeOutGraphic, Mathf.Lerp(fadeOutStartAlpha, 0f, alpha));
                }

                if (fadeInGraphic != null)
                {
                    UIUtil.SetGraphicAlpha(fadeInGraphic, Mathf.Lerp(0f, fadeInTargetAlpha, alpha));
                }

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            if (fadeOutGraphic != null)
            {
                UIUtil.SetGraphicAlpha(fadeOutGraphic, 0f);
            }

            if (fadeInGraphic != null)
            {
                UIUtil.SetGraphicAlpha(fadeInGraphic, fadeInTargetAlpha);
            }
        }
    }
}
