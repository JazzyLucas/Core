using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzyLucas.Packaging
{
    [CreateAssetMenu(fileName = "PackageGeneratorConfig", menuName = "JazzyLucas.Packaging/PackageGeneratorConfig")]
    public class PackageGeneratorConfigSO : ScriptableObject
    {
        [field: Tooltip("Path to folder to recursively package.")]
        public string AssetsPath = "Assets/Plugins/JazzyLucas";
        
        [field: Tooltip("Path to the package.json.")]
        public string PackageJsonPath = "Assets/Plugins/JazzyLucas/package.json";
        
        [field: Tooltip("Path to where the generated unitypackage file will go.")]
        public string OutputPath = "Builds";
    }
}
