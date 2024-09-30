using System;
using System.Collections.Generic;
using JazzyLucas.Core.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
// ReSharper disable CollectionNeverUpdated.Global

namespace JazzyLucas.Core
{
    public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action OnHover; 
        public event Action OnUnhover;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerEnter || eventData.reentered)
                OnHover?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnUnhover?.Invoke();
        }
    }
}