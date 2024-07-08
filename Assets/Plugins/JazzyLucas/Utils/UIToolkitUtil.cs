using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace JazzyLucas.Utils
{
    public static class UIToolkitUtil
    {
        public static void TranslateElementToMouse(RectTransform rootCanvasTransform, RectTransform element, Vector2 screenMousePosition)
        {
            var canvas = rootCanvasTransform.GetComponentInParent<Canvas>();
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                // For WorldSpace canvases, convert screen to world point
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rootCanvasTransform, screenMousePosition, canvas.worldCamera, out var worldPoint))
                {
                    element.position = worldPoint;
                }
            }
            else
            {
                // For ScreenSpace canvases
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rootCanvasTransform, screenMousePosition, canvas.worldCamera, out var localPoint))
                {
                    element.localPosition = localPoint;
                }
            }
        }

        public static IEnumerable<Button> GetAllButtons(string tagName)
        {
            return GameObject.FindGameObjectsWithTag(tagName).Select(obj => obj.GetComponent<Button>()).Where(button => button != null);
        }
    }
}