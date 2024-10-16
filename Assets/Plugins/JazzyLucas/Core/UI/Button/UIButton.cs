using System.Collections;
using JazzyLucas.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace JazzyLucas.Core
{
    public class UIButton : UIBehavior
    {
        [field: SerializeField] public Clickable Clickable { get; private set; }
        [field: SerializeField] public Hoverable Hoverable { get; private set; }
        [field: SerializeField] public Graphic Icon { get; set; }
        
        // TODO: is this just a more-or-less datastructure, or an actual behavior?
    }
}
