using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace JazzyLucas.Packaging
{
    public static class ExportTools
    {
        private const string NAME = "JazzyLucas_Core";
        private static readonly string VERSION = System.Environment.GetCommandLineArgs().FirstOrDefault(arg => arg.StartsWith("version="))?.Split('=')[1]; // (For use with GitHub Actions)
        private static readonly string FILE_NAME = $"{NAME}_v{VERSION}.unitypackage";
        private static readonly string[] CONTENT_PATHS = { "Assets/Plugins/JazzyLucas" };
        
        [MenuItem("JazzyLucas/Packaging/Export")]
        private static void Export()
        {
            AssetDatabase.ExportPackage(CONTENT_PATHS, FILE_NAME, ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
            Debug.Log($"Exported {FILE_NAME}");
        }
    }
}
