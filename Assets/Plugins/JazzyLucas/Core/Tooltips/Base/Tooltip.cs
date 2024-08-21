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

            var offset = new Vector2(tooltipWidth/1.5f, -tooltipHeight/1.5f);

            // Initial position (southeast of the cursor)
            var initialPosition = cursorPosition + offset;
            
            // Clamp the X position to ensure the tooltip is within screen width
            float finalX = Mathf.Clamp(initialPosition.x, 0, screenWidth - tooltipWidth/2);

            // Clamp the Y position to ensure the tooltip is within screen height
            float finalY = Mathf.Clamp(initialPosition.y, tooltipHeight/2, screenHeight);

            // Apply the clamped position to the tooltip
            Transform.position = new Vector2(finalX, finalY);
        }
    }
}