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
        public event Action<Behavior> BSpawnedEvent;
        public void InvokeBSpawnedEvent(Behavior B) => BSpawnedEvent?.Invoke(B);
        public event Action<Behavior> BDestroyedEvent;
        public void InvokeBDestroyedEvent(Behavior B) => BDestroyedEvent?.Invoke(B);
        #endregion
    }
}