using System;
using UnityEngine;

namespace JazzyLucas.Core
{
    public abstract class Container : ScriptableObject
    {
        #region EVENTS
        public event Action<Behavior<Blueprint, Data<Blueprint>>> BSpawnedEvent;
        public void InvokeBSpawnedEvent<TBlueprint, TData>(Behavior<TBlueprint, TData> B)
            where TBlueprint : Blueprint
            where TData : Data<TBlueprint>
        {
            BSpawnedEvent?.Invoke(B as Behavior<Blueprint, Data<Blueprint>>);
        }

        public event Action<Behavior<Blueprint, Data<Blueprint>>> BDestroyedEvent;
        public void InvokeBDestroyedEvent<TBlueprint, TData>(Behavior<TBlueprint, TData> B)
            where TBlueprint : Blueprint
            where TData : Data<TBlueprint>
        {
            BDestroyedEvent?.Invoke(B as Behavior<Blueprint, Data<Blueprint>>);
        }
        #endregion
    }
}