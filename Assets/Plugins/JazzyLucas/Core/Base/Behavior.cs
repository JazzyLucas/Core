using UnityEngine;

namespace JazzyLucas.Core
{
    [System.Serializable]
    public abstract class Behavior : MonoBehaviour
    {
        protected abstract Container BaseContainer { get; }
        
        protected abstract Blueprint BaseBlueprint { get; }
        
        // TODO: BaseData?
        // protected abstract Data BaseData { get; }
        
        protected virtual void Start()
        {
            BaseContainer.InvokeBSpawnedEvent(this);
        }
        internal void Destroy()
        {
            BaseContainer.InvokeBDestroyedEvent(this);
        }
    }
}