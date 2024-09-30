using System.Collections;
using System.Collections.Generic;
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
        private Coroutine colorChangeCoroutine;

        private void Awake()
        {
            originalColor = graphic.color;
            // Preserve the transparency
            var color = hoverColor;
            color.a = graphic.color.a;
            hoverColor = color;

            hoverable.OnHover += () =>
            {
                if (colorChangeCoroutine != null)
                    StopCoroutine(colorChangeCoroutine);

                colorChangeCoroutine = StartCoroutine(AnimateColorChange(graphic.color, hoverColor));
            };

            hoverable.OnUnhover += () =>
            {
                if (colorChangeCoroutine != null)
                    StopCoroutine(colorChangeCoroutine);

                colorChangeCoroutine = StartCoroutine(AnimateColorChange(graphic.color, originalColor));
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
