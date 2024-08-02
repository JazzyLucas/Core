using System;
using UnityEngine;

namespace JazzyLucas.Core
{
    public abstract class Container : ScriptableObject
    {
        public static CoreContainer CoreContainer { get; private set; }
        internal static void Inject(CoreContainer coreContainer) => CoreContainer = coreContainer;
        protected Manager BaseManager { get; private set; }
        public void Inject(Manager manager) => BaseManager = manager;
        
        #region EVENTS
        public event Action<Behavior<Blueprint, Data<Blueprint>>> BSpawnedEvent;
        public void InvokeBSpawnedEvent<TBlueprint, TData>(Behavior<TBlueprint, TData> B)
            where TBlueprint : Blueprint
            where TData : Data<TBlueprint>
        {
            // Casting the event arguments to match the event signature
            BSpawnedEvent?.Invoke(B as Behavior<Blueprint, Data<Blueprint>>);
        }

        public event Action<Behavior<Blueprint, Data<Blueprint>>> BDestroyedEvent;
        public void InvokeBDestroyedEvent<TBlueprint, TData>(Behavior<TBlueprint, TData> B)
            where TBlueprint : Blueprint
            where TData : Data<TBlueprint>
        {
            // Casting the event arguments to match the event signature
            BDestroyedEvent?.Invoke(B as Behavior<Blueprint, Data<Blueprint>>);
        }
        #endregion
    }
}