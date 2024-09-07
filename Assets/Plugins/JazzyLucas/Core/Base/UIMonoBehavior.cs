using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzyLucas.Core
{
    /// <summary>
    /// Toggle-able UI without de-activating scripts.
    /// </summary>
    /// <remarks>
    /// Use within a Canvas.
    /// </remarks>
    [RequireComponent(typeof(CanvasGroup))]
    public class UIMonoBehavior : MonoBehaviour
    {
        [field:HideInInspector] public event Action<bool> OnVisibilityChanged;
        [field:HideInInspector] public bool IsVisible => _canvasGroup.alpha > 0.1;
        
        private CanvasGroup _canvasGroup;
        
        private void Awake()
        {
            OnAwake();
        }

        /// <summary>
        /// Called in Unity's Awake.
        /// </summary>
        /// <remarks>
        /// Please do base.OnAwake();
        /// </remarks>
        protected virtual void OnAwake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetVisibility(bool value)
        {
            _canvasGroup.alpha = value ? 1 : 0;
            _canvasGroup.blocksRaycasts = value;
            _canvasGroup.interactable = value;
            OnVisibilityChanged?.Invoke(value);
        }
    }
}
