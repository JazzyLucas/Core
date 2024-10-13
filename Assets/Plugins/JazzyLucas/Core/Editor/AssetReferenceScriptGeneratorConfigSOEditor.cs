using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using UnityEditor;
using UnityEngine;

namespace JazzyLucas.Core.Editor
{
    [CustomEditor(typeof(AssetReferenceScriptGeneratorConfigSO))]
    public class AssetReferenceScriptGeneratorConfigSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("In the Inspector, right-click on a folder and choose 'Copy Path' to get the path.", MessageType.Info);
            
            DrawDefaultInspector();
        }
    }
}
