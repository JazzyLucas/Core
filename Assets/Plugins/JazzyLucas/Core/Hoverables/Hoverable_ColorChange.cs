using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace JazzyLucas.Core
{
    public class Hoverable_ColorChange : MonoBehaviour
    {
        [field: SerializeField] public Hoverable hoverable { get; private set; }
        [field: SerializeField] public Graphic graphic { get; private set; }
        [field: SerializeField] public Color hoverColor { get; private set; }
        [field: SerializeField] public float animationDuration { get; private set; } = 0.2f;

        private Color originalColor;
        private Coroutine coroutine;

        private void Awake()
        {
            originalColor = graphic.color;
            // Have the hoverColor use the original alpha of the graphic
            var newHoverColor = hoverColor;
            newHoverColor.a = graphic.color.a;
            hoverColor = newHoverColor;

            hoverable.OnHover += () =>
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                coroutine = StartCoroutine(AnimateColorChange(graphic.color, hoverColor));
            };

            hoverable.OnUnhover += () =>
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                coroutine = StartCoroutine(AnimateColorChange(graphic.color, originalColor));
            };
        }

        private IEnumerator AnimateColorChange(Color startColor, Color targetColor)
        {
            float timeElapsed = 0f;

            while (timeElapsed < animationDuration)
            {
                graphic.color = Color.Lerp(startColor, targetColor, timeElapsed / animationDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            graphic.color = targetColor;
        }
    }
}
