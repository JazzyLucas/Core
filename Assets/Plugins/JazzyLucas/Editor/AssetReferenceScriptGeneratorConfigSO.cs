using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace JazzyLucas.Editor
{
    public class AssetReferenceScriptGeneratorConfigSO : ScriptableObject
    {
        public string OutputPath = "Assets/Prefabs/Generated";
        public string AssetsPath = "Assets/Prefabs";
        public string FileName = "Prefabs";
        public string ClassName = "Prefabs";
    }
}
