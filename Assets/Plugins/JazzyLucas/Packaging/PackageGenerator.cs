using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JazzyLucas.Core.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Packaging
{
    public static class PackageGenerator
    {
        private const string DEFAULT_CONFIG_SO_PATH = "Assets/Plugins/JazzyLucas/Packaging/PackageGeneratorConfig.asset";

        [MenuItem("Tools/JazzyLucas.Packaging/GeneratePackage")]
        public static void GeneratePackage()
        {
            L.Log($"Running Default GeneratePackage()");

            if (!LoadOrCreateDefaultConfig(out var config))
            {
                L.Log(LogSeverity.ERROR, "Failed to create or load the default config.");
                return;
            }

            GeneratePackage(config);
        }
        
        public static void GeneratePackage(PackageGeneratorConfigSO config)
        {
            var assetsPath = config.AssetsPath;
            var packageJsonPath = config.PackageJsonPath;
            var outputPath = config.OutputPath;
            
            if (!File.Exists(packageJsonPath))
            {
                L.Log("package.json not found!");
                return;
            }
            
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);
            
            L.Log($"Starting generation of package from {assetsPath} to {outputPath}");

            var packageJson = JsonUtility.FromJson<PackageJson>(File.ReadAllText(packageJsonPath));
            var versionParts = packageJson.version.Split('.');
            var patchVersion = int.Parse(versionParts[2]);
            versionParts[2] = (patchVersion + 1).ToString();
            var newVersion = string.Join(".", versionParts);

            packageJson.version = newVersion;
            File.WriteAllText(packageJsonPath, JsonUtility.ToJson(packageJson, true));

            var packageName = packageJson.name.Replace("com.", "").Replace(".", "_");
            var fileName = $"{packageName}_v{newVersion}.unitypackage";
            var buildsFolderPath =
                Path.Combine(Path.GetDirectoryName(Application.dataPath) ?? throw new InvalidOperationException(),
                    outputPath);

            if (!Directory.Exists(buildsFolderPath))
                Directory.CreateDirectory(buildsFolderPath);

            var filePath = Path.Combine(buildsFolderPath, fileName);

            AssetDatabase.ExportPackage(assetsPath, filePath,
                ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);

            var folderUrl = "file:///" + buildsFolderPath.Replace("\\", "/");
            L.Log($"<color=white>Exported </color>" +
                  $"<color=green>{fileName}</color>" +
                  $"<color=white> to </color>" +
                  $"<color=blue><a href=\"{folderUrl}\">{filePath}</a></color>");
        }
        
        private static bool LoadOrCreateDefaultConfig(out PackageGeneratorConfigSO config)
        {
            config = AssetDatabase.LoadAssetAtPath<PackageGeneratorConfigSO>(DEFAULT_CONFIG_SO_PATH);

            if (config is null)
            {
                L.Log(LogSeverity.ERROR, $"Config SO not found at {DEFAULT_CONFIG_SO_PATH}. Creating default config file.");

                config = ScriptableObject.CreateInstance<PackageGeneratorConfigSO>();
                AssetDatabase.CreateAsset(config, DEFAULT_CONFIG_SO_PATH);
                AssetDatabase.RenameAsset(DEFAULT_CONFIG_SO_PATH, "PackageGeneratorConfig"); // TODO: just get the name by trimming the end?
                AssetDatabase.SaveAssets();
                
                L.Log($"Default config SO created at {DEFAULT_CONFIG_SO_PATH}");
                L.Log($"Please verify config, then run again.");
                return false;
            }

            L.Log($"Config loaded: Prefabs Path = {config.AssetsPath}, Output Path = {config.OutputPath}");
            return true;
        }
    }

    [Serializable]
    public struct PackageJson
    {
        public string name;
        public string version;
        public string displayName;
        public string unity;
    }
}