using System;
using JazzyLucas.Core.Utils;
using UnityEngine;

namespace JazzyLucas.Core
{
    public abstract class Tooltip : MonoBehaviour
    {
        [field: SerializeField] public RectTransform Transform { get; private set; }

        private void Awake()
        {
            if (!Transform)
                Transform = transform as RectTransform;
        }

        private void LateUpdate()
        {
            Vector2 cursorPosition = UnityEngine.Input.mousePosition;

            var screenWidth = Screen.width;
            var screenHeight = Screen.height;
            
            var tooltipWidth = Transform.rect.width;
            var tooltipHeight = Transform.rect.height;

            var offset = new Vector2(tooltipWidth / 1.5f, -tooltipHeight / 1.5f);
            var initialPosition = cursorPosition + offset;

            // Check if the tooltip would go off the right or bottom edge of the screen
            bool offRight = initialPosition.x + tooltipWidth / 2 > screenWidth;
            bool offBottom = initialPosition.y - tooltipHeight / 2 < 0;

            // Flip the offset direction if needed
            if (offRight) 
                offset.x = -offset.x;
            if (offBottom) 
                offset.y = -offset.y;

            var finalPosition = cursorPosition + offset;
            Transform.position = new Vector2(finalPosition.x, finalPosition.y);
        }
    }
}