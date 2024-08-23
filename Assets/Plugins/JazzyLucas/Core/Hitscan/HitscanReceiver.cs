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

        public virtual void InvokeOnHitscan(InputData data)
        {
            OnHitscan?.Invoke(data);
        }
        
        [field: SerializeField] public event Action OnUnHitscan;

        public virtual void InvokeOnUnHitscan()
        {
            OnUnHitscan?.Invoke();
        }
    }
}