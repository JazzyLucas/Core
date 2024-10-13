using System;
using System.Collections.Generic;
using JazzyLucas.Core.Input;
using UnityEngine;
using UnityEngine.Events;

namespace JazzyLucas.Core
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class HitscanReceiver : MonoBehaviour
    {
        [field: SerializeField] public event Action<InputData> OnHitscan;

        public void InvokeOnHitscan(InputData data)
        {
            OnHitscan?.Invoke(data);
        }
        
        [field: SerializeField] public event Action OnUnHitscan;

        public void InvokeOnUnHitscan()
        {
            OnUnHitscan?.Invoke();
        }
    }
}