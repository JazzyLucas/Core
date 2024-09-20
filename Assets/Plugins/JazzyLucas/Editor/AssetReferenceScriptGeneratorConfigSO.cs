using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace JazzyLucas.Editor
{
    [CreateAssetMenu(fileName = "AssetReferenceScriptGeneratorConfig", menuName = "JazzyLucas.Editor/AssetReferenceScriptGeneratorConfig")]
    public class AssetReferenceScriptGeneratorConfigSO : ScriptableObject
    {
        [field: Tooltip("Path to folder to recursively scan for assets.")]
        public string AssetsPath = "Assets/Prefabs";
        
        [field: Tooltip("What paths to ignore when scanning.")]
        public string[] Blacklist = {"Assets/Prefabs/Generated"};
        
        [field: Tooltip("Where to create/update generated script file.")]
        public string OutputPath = "Assets/Prefabs/Generated";
        
        [field: Tooltip("File name of the generated script.")]
        public string FileName = "Prefabs";
        
        [field: Tooltip("Class name of the generated script.")]
        public string ClassName = "Prefabs";
    }
}
