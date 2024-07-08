using UnityEngine;

namespace JazzyLucas.Core
{
    [CreateAssetMenu(fileName = "CoreContainer", menuName = "Container/Core", order = 1)]
    public class CoreContainer : ScriptableObject
    {
        public CoreManager CoreManager { get; set; }
    }
}