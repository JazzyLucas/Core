using System.Collections;
using System.Collections.Generic;
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
            originalAlpha = hoverGraphic.color.a;
            UIUtil.SetGraphicAlpha(hoverGraphic, 0f);
            hoverGraphic.gameObject.SetActive(false);

            hoverable.OnHover += () =>
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                hoverGraphic.gameObject.SetActive(true);
                coroutine = StartCoroutine(AnimateGraphicChange(originalGraphic, hoverGraphic, true));
            };

            hoverable.OnUnhover += () =>
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                originalGraphic.gameObject.SetActive(true);
                coroutine = StartCoroutine(AnimateGraphicChange(hoverGraphic, originalGraphic, false));
            };
        }

        private IEnumerator AnimateGraphicChange(Graphic fadeOutGraphic, Graphic fadeInGraphic, bool isHovering)
        {
            var timeElapsed = 0f;
            var fadeOutStartAlpha = fadeOutGraphic.color.a;
            var fadeInTargetAlpha = originalAlpha;

            while (timeElapsed < animationDuration)
            {
                float alpha = timeElapsed / animationDuration;

                UIUtil.SetGraphicAlpha(fadeOutGraphic, Mathf.Lerp(fadeOutStartAlpha, 0f, alpha));
                UIUtil.SetGraphicAlpha(fadeInGraphic, Mathf.Lerp(0f, fadeInTargetAlpha, alpha));

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Set final alpha values explicitly
            UIUtil.SetGraphicAlpha(fadeOutGraphic, 0f);
            UIUtil.SetGraphicAlpha(fadeInGraphic, fadeInTargetAlpha);

            fadeOutGraphic.gameObject.SetActive(false);
        }
    }
}