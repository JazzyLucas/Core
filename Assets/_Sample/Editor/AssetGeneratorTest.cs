using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using L = JazzyLucas.Core.Utils.Logger;

public class AssetGeneratorTest : UnityEditor.Editor
{
    [MenuItem("Tools/_Sample/Generate Asset References")]
    public virtual void Generate()
    {
        Debug.Log("Base prefab generator logic. This can be overridden.");
    }
}