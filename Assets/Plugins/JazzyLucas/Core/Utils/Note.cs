using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzyLucas.Core.Utils
{
    [AddComponentMenu("JazzyLucas/Utilities/Note")]
    public class Note : MonoBehaviour
    {
        [field: TextArea(3, 10)] 
        [field: SerializeField] public string note { get; private set; } = "Add your note here!";
    }
}
