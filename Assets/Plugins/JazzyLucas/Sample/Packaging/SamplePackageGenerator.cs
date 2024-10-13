using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using JazzyLucas.Core.Editor;
using JazzyLucas.Packaging;
using UnityEditor;
using UnityEngine;
using L = JazzyLucas.Core.Utils.Logger;

public class SamplePackageGenerator : UnityEditor.Editor
{
    private const string SAMPLE_CONFIG_SO_PATH = "Assets/Plugins/JazzyLucas/Sample/Packaging/PackageGeneratorConfig.asset";

    [MenuItem("Tools/JazzyLucas.Sample/GenerateSamplePackage", false, 0)]
    public static void GenerateSamplePackage()
    {
        var config = AssetDatabase.LoadAssetAtPath<PackageGeneratorConfigSO>(SAMPLE_CONFIG_SO_PATH);
        PackageGenerator.GeneratePackage(config);
    }
}