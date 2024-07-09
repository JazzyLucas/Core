using System;
using UnityEngine;
using JazzyLucas.Core.Utils;

namespace JazzyLucas.Core
{
    public abstract class Manager : Singleton<MonoBehaviour>
    {
        /// <summary>
        /// Typically overriden with a lambda to a Serialized container.
        /// </summary>
        protected abstract Container BaseContainer { get; }

        public virtual void Awake()
        {
            CoreManager.Instance.AddContainer(BaseContainer);
            BaseContainer.Inject(this);
        }
    }
}