using UnityEngine;
using UnityEditor;

public static class Prefabs
{
    public static class TestSubdirectory
    {
        public static readonly GameObject AnotherTestPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TestSubdirectory/AnotherTestPrefab.prefab");
    }
    public static readonly GameObject TestPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TestPrefab.prefab");
}
