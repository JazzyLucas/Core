using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JazzyLucas.Core.Utils;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    public class CoreManager : Singleton<CoreManager>
    {
        [field: SerializeField] public CoreContainer CoreContainer { get; private set; }
        public HashSet<Container> Containers { get; } = new();
        public Container GetContainer(Type T)
        {
            var found = Containers.FirstOrDefault(c => c.GetType() == T);
            if (found is not null) return found;
            {
                L.Log($"GetContainer() found null for {T.Name}!!! Will be trying to find via a new Init() call, hang tight!");
                Init();
                found = Containers.FirstOrDefault(c => c.GetType() == T);
            }
            return found;
        }
        public void AddContainer(Container container)
        {
            Containers.Remove(container);
            Containers.Add(container);
            Container.Inject(CoreContainer);
        }
        protected override void Init()
        {
            if (Initialized) return;
            foreach (var manager in FindObjectsByType<Manager>(FindObjectsSortMode.None))
                manager.Awake();
        }
    }
}