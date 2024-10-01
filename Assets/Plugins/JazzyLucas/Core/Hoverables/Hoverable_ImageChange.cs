using System.Collections;
using JazzyLucas.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace JazzyLucas.Core
{
    public class Hoverable_ImageChange : MonoBehaviour
    {
        [field: SerializeField] public Hoverable hoverable { get; private set; }
        [field: SerializeField] public Graphic originalGraphic { get; private set; }
        [field: SerializeField] public Graphic hoverGraphic { get; private set; }
        [field: SerializeField] public float animationDuration { get; private set; } = 0.1f;

        private float originalAlpha;
        private Coroutine coroutine;

        private void Awake()
        {
            // Handle null checks for hoverGraphic and originalGraphic
            if (hoverGraphic != null)
            {
                originalAlpha = hoverGraphic.color.a;
                UIUtil.SetGraphicAlpha(hoverGraphic, 0f);
                hoverGraphic.gameObject.SetActive(false);
            }

            hoverable.OnHover += () =>
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                if (hoverGraphic != null)
                {
                    hoverGraphic.gameObject.SetActive(true);
                }

                coroutine = StartCoroutine(AnimateGraphicChange(originalGraphic, hoverGraphic, true));
            };

            hoverable.OnUnhover += () =>
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                if (originalGraphic != null)
                {
                    originalGraphic.gameObject.SetActive(true);
                }

                coroutine = StartCoroutine(AnimateGraphicChange(hoverGraphic, originalGraphic, false));
            };
        }

        private IEnumerator AnimateGraphicChange(Graphic fadeOutGraphic, Graphic fadeInGraphic, bool isHovering)
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
                fadeOutGraphic.gameObject.SetActive(false);
            }

            if (fadeInGraphic != null)
            {
                UIUtil.SetGraphicAlpha(fadeInGraphic, fadeInTargetAlpha);
            }
        }
    }
}
