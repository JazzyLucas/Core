using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Editor;
using UnityEditor;
using UnityEngine;
using L = JazzyLucas.Core.Utils.Logger;

public class AssetGeneratorTest : UnityEditor.Editor
{
    private const string SAMPLE_CONFIG_SO_PATH = "Assets/_Sample/Editor/AssetReferenceScriptGeneratorConfig.asset";

    [MenuItem("Tools/Sample/Run Sample GenerateAssetReferences", false, 0)]
    public static void GeneratePrefabReferences()
    {
        var config = AssetDatabase.LoadAssetAtPath<AssetReferenceScriptGeneratorConfigSO>(SAMPLE_CONFIG_SO_PATH);
        AssetReferenceScriptGenerator.GeneratePrefabReferences(config);
    }
}