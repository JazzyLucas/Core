using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextHologram : Hologram
{
    [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
    [field: SerializeField] public Image TextBackground { get; private set; }
    
    // TODO: resizing image based on text
}
