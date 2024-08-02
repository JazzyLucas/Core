using System;
using UnityEngine;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    [System.Serializable]
    public abstract class Behavior<TBlueprint, TData> : MonoBehaviour
        where TBlueprint : Blueprint
        where TData : Data<TBlueprint>
    {
        protected abstract Container BaseContainer { get; }
        public virtual TBlueprint Blueprint { get; protected set; }
        public virtual TData Data { get; protected set; }

        private bool initialized;
        public void Init(TBlueprint blueprint)
        {
            Blueprint = blueprint;
            Data = Activator.CreateInstance<TData>();
            Data.Init(blueprint);
            initialized = true;
        }
        
        protected virtual void Start()
        {
            if (!initialized)
                L.Log($"{name} called Start() but was not initialized.");
            BaseContainer.InvokeBSpawnedEvent(this);
        }
        
        internal void Destroy()
        {
            BaseContainer.InvokeBDestroyedEvent(this);
        }
    }
}