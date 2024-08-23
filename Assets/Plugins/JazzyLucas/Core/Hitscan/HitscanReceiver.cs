using System;
using UnityEngine;
using UnityEngine.Events;

namespace JazzyLucas.Core
{
    [DisallowMultipleComponent]
    public class HitscanReceiver : MonoBehaviour
    {
        [field: SerializeField] public event Action<HitscanData> OnHitscan;
        public virtual void InvokeOnHitscan(HitscanData data)
        {
            OnHitscan?.Invoke(data);
        }
    }
}