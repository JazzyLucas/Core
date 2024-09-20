using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Packaging
{
    public static class ExportTools
    {
        private const string PACKAGE_JSON_PATH = "Assets/Plugins/JazzyLucas/package.json";
        private const string EXPORT_PATH = "Assets/Plugins/JazzyLucas";
        private const string BUILDS_FOLDER_NAME = "Builds";

        [MenuItem("Tools/JazzyLucas.Packaging/Export Package")]
        public static void Export()
        {
            if (!File.Exists(PACKAGE_JSON_PATH))
            {
                L.Log("package.json not found!");
                return;
            }

            var packageJson = JsonUtility.FromJson<PackageJson>(File.ReadAllText(PACKAGE_JSON_PATH));
            var versionParts = packageJson.version.Split('.');
            var patchVersion = int.Parse(versionParts[2]);
            versionParts[2] = (patchVersion + 1).ToString();
            var newVersion = string.Join(".", versionParts);

            packageJson.version = newVersion;
            File.WriteAllText(PACKAGE_JSON_PATH, JsonUtility.ToJson(packageJson, true));

            var packageName = packageJson.name.Replace("com.", "").Replace(".", "_");
            var fileName = $"{packageName}_v{newVersion}.unitypackage";
            var buildsFolderPath =
                Path.Combine(Path.GetDirectoryName(Application.dataPath) ?? throw new InvalidOperationException(),
                    BUILDS_FOLDER_NAME);

            if (!Directory.Exists(buildsFolderPath))
                Directory.CreateDirectory(buildsFolderPath);

            var filePath = Path.Combine(buildsFolderPath, fileName);

            AssetDatabase.ExportPackage(EXPORT_PATH, filePath,
                ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);

            // Create a clickable hyperlink in the Unity Console
            var folderUrl = "file:///" + buildsFolderPath.Replace("\\", "/");
            L.Log($"<color=white>Exported </color>" +
                  $"<color=green>{fileName}</color>" +
                  $"<color=white> to </color>" +
                  $"<color=blue><a href=\"{folderUrl}\">{filePath}</a></color>");
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