using System;
using System.Collections.Generic;
using JazzyLucas.Core.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
// ReSharper disable CollectionNeverUpdated.Global

namespace JazzyLucas.Core
{
    public class Clickable : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnClick; 
        public event Action OnLeftClick;
        public event Action OnRightClick;
        public event Action OnMiddleClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.pointerClick)
                OnClick?.Invoke();
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    OnLeftClick?.Invoke();
                    break;
                case PointerEventData.InputButton.Right:
                    OnRightClick?.Invoke();
                    break;
                case PointerEventData.InputButton.Middle:
                    OnMiddleClick?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}