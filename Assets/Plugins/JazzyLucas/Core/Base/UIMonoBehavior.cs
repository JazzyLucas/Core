using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzyLucas.Core
{
    /// <summary>
    /// Use this class to make toggle-able UI without de-activating scripts.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UIMonoBehavior : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        public event Action<bool> OnVisibilityChanged;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetVisibility(bool value)
        {
            _canvasGroup.alpha = value ? 0 : 1;
            _canvasGroup.blocksRaycasts = value;
            _canvasGroup.interactable = value;
            OnVisibilityChanged?.Invoke(value);
        }
    }
}
