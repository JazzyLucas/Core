using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JazzyLucas.Core.Utils;
using UnityEditor;
using UnityEngine;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Editor
{
    public class AssetReferenceScriptGenerator : UnityEditor.Editor
    {
        private const string CONFIG_SO_PATH = "Assets/Plugins/JazzyLucas/Editor/AssetReferenceScriptGeneratorConfig.asset";

        private static AssetReferenceScriptGeneratorConfigSO config;
        private static string assetsPath => config.AssetsPath;
        private static string outputPath => config.OutputPath;
        private static string fileName => config.FileName;
        private static string className => config.ClassName;

        [MenuItem("Tools/JazzyLucas/Generate Asset References")]
        public static void GeneratePrefabReferences()
        {
            L.Log($"Running GeneratePrefabReferences()");
            
            if (!LoadConfig())
                return;

            L.Log($"Starting generation of prefab references from {assetsPath} to {outputPath}");

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
                L.Log($"Created output directory at {outputPath}");
            }

            var sb = new StringBuilder();
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using UnityEditor;");
            sb.AppendLine();
            sb.AppendLine($"public static class {className}");
            sb.AppendLine("{");

            GeneratePrefabClasses(assetsPath, sb, 1);

            sb.AppendLine("}");

            var outputFilePath = Path.Combine(outputPath, $"{fileName}.cs");
            File.WriteAllText(outputFilePath, sb.ToString());
            L.Log($"Prefab references written to {outputFilePath}");

            AssetDatabase.Refresh();
        }

        private static void GeneratePrefabClasses(string directory, StringBuilder sb, int indentLevel)
        {
            var indent = new string(' ', indentLevel * 4);

            foreach (var subdirectory in Directory.GetDirectories(directory))
            {
                if (Directory.GetFiles(subdirectory, "*.prefab").Any() || Directory.GetDirectories(subdirectory).Any())
                {
                    var folderName = CapitalizeFirstLetter(Path.GetFileName(subdirectory).Replace(" ", "_"));
                    sb.AppendLine($"{indent}public static class {folderName}");
                    sb.AppendLine($"{indent}{{");
                    GeneratePrefabClasses(subdirectory, sb, indentLevel + 1);
                    sb.AppendLine($"{indent}}}");
                    L.Log($"Added class for directory: {subdirectory}");
                }
                else
                {
                    L.Log($"Skipped empty directory: {subdirectory}");
                }
            }

            foreach (var file in Directory.GetFiles(directory, "*.prefab"))
            {
                var fileName = CapitalizeFirstLetter(Path.GetFileNameWithoutExtension(file).Replace(" ", "_"));
                var relativePath = file.Replace(Application.dataPath, "Assets").Replace("\\", "/");
                sb.AppendLine($"{indent}public static readonly GameObject {fileName} = AssetDatabase.LoadAssetAtPath<GameObject>(\"{relativePath}\");");
                L.Log($"Added prefab reference for file: {file} as {fileName}");
            }
        }

        private static string CapitalizeFirstLetter(string input) => string.IsNullOrEmpty(input) ? input : char.ToUpper(input[0]) + input[1..];

        private static bool LoadConfig()
        {
            config = AssetDatabase.LoadAssetAtPath<AssetReferenceScriptGeneratorConfigSO>(CONFIG_SO_PATH);

            if (config is null)
            {
                L.Log(LogSeverity.ERROR, $"Config SO not found at {CONFIG_SO_PATH}. Creating default config file.");

                config = CreateInstance<AssetReferenceScriptGeneratorConfigSO>();
                AssetDatabase.CreateAsset(config, CONFIG_SO_PATH);
                AssetDatabase.SaveAssets();

                L.Log($"Default config SO created at {CONFIG_SO_PATH}");
                L.Log($"Please verify config, then run again.");
                return false;
            }

            L.Log($"Config loaded: Prefabs Path = {assetsPath}, Output Path = {outputPath}");
            return true;
        }
    }
}
