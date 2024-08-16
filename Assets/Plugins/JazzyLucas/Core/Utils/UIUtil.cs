using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core.Utils
{
    public class UIUtil : MonoBehaviour
    {
        public static void AnchorToWorldSpacePoint(Transform target, RectTransform rectTransform, Camera camera = null)
        {
            if (target == null)
            {
                L.Log("target is null.");
                return;
            }
            if (rectTransform == null)
            {
                L.Log("rectTransform is null.");
                return;
            }
            var cam = camera ?? Camera.main;
            if (cam == null)
            {
                L.Log("camera is null and can't be found.");
                return;
            }
            var screenPosition = cam.WorldToScreenPoint(target.position);
            rectTransform.position = screenPosition;
        }
    }
}
