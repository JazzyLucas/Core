using System;
using UnityEngine;
using JazzyLucas.Core.Utils;

namespace JazzyLucas.Core
{
    public abstract class Manager<TContainer> : Singleton<Manager<TContainer>> where TContainer : Container
    {
        [field: SerializeField] public TContainer Container;
    }
}