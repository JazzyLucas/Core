using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using JazzyLucas.Core.Editor;
using UnityEditor;
using UnityEngine;
using L = JazzyLucas.Core.Utils.Logger;

public class AssetGeneratorTest : UnityEditor.Editor
{
    private const string SAMPLE_CONFIG_SO_PATH = "Assets/Plugins/JazzyLucas/Sample/Editor/AssetReferenceScriptGeneratorConfig.asset";

    [MenuItem("Tools/JazzyLucas.Sample/GenerateAssetReferences", false, 0)]
    public static void GeneratePrefabReferences()
    {
        var config = AssetDatabase.LoadAssetAtPath<AssetReferenceScriptGeneratorConfigSO>(SAMPLE_CONFIG_SO_PATH);
        AssetReferenceScriptGenerator.GeneratePrefabReferences(config);
    }
}