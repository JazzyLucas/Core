using System;
using UnityEngine;
using JazzyLucas.Core.Input;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    public abstract class Controller : MonoBehaviour
    {
        [field: SerializeField] public bool InitOnAwake { get; private set; } = false;
        
        protected InputPoller inputPoller { get; private set; }
        public bool Initialized { get; private set; } = false;
        
        private void Awake()
        {
            inputPoller = new();
            if (InitOnAwake)
                Init();
        }

        private void Update()
        {
            if (!Initialized)
            {
                L.Log($"{this} called Update() but is not Initialized. Abandoning Process.");
                return;
            }
            
            Process();
        }

        /// <summary>
        /// Called in Unity's Awake if InitOnAwake is true.
        /// Otherwise, you will need to manually call this.
        /// </summary>
        /// <remarks>
        /// When overriding, please do a base.OnAwake() first.
        /// </remarks>
        public virtual void Init()
        {
            Initialized = true;
        }

        protected abstract void Process();
    }
}