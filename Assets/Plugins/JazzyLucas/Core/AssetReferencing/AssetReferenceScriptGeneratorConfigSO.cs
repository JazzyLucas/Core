using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace JazzyLucas.Core
{
    [CreateAssetMenu(fileName = "AssetReferenceScriptGeneratorConfig", menuName = "JazzyLucas.Core/AssetReferenceScriptGeneratorConfig")]
    public class AssetReferenceScriptGeneratorConfigSO : ScriptableObject
    {
        [field: Tooltip("Path to folder to recursively scan for assets.")]
        public string AssetsPath = "Assets";
        
        [field: Tooltip("What paths to ignore when scanning.")]
        public string[] Blacklist = {"Assets/TextMesh Pro"};
        
        [field: Tooltip("Where to create/update generated script file.")]
        public string OutputPath = "Assets/Generated";
        
        [field: Tooltip("File name of the generated script.")]
        public string FileName = "AssetReferences";
        
        [field: Tooltip("Class name of the generated script.")]
        public string ClassName = "AssetReferences";

        [field: Tooltip("What asset types do you want to generate static references for?")]
        public UnityAssetTypeFlags ValidAssetTypes = new(Prefabs: true,
            Materials: true,
            ScriptableObjects: true,
            Textures: true,
            Meshes: true,
            Animations: true,
            AudioClips: true,
            Shaders: true,
            Fonts: true,
            VideoClips: true,
            Sprites: true,
            ParticleSystems: true,
            NavMeshData: false,
            LightingData: false,
            PhysicsMaterials: false,
            Terrains: false,
            RenderTextures: true);
    }

    [System.Serializable]
    public struct UnityAssetTypeFlags
    {
        public bool Prefabs;
        public bool Materials;
        public bool ScriptableObjects;
        public bool Textures;
        public bool Meshes;
        public bool Animations;
        public bool AudioClips;
        public bool Shaders;
        public bool Fonts;
        public bool VideoClips;
        public bool Sprites;
        public bool ParticleSystems;
        public bool NavMeshData;
        public bool LightingData;
        public bool PhysicsMaterials;
        public bool Terrains;
        public bool RenderTextures;
        
        public UnityAssetTypeFlags(bool Prefabs, bool Materials, bool ScriptableObjects, bool Textures, bool Meshes,
            bool Animations, bool AudioClips, bool Shaders, bool Fonts, bool VideoClips, bool Sprites,
            bool ParticleSystems, bool NavMeshData, bool LightingData, bool PhysicsMaterials, bool Terrains,
            bool RenderTextures)
        {
            this.Prefabs = Prefabs;
            this.Materials = Materials;
            this.ScriptableObjects = ScriptableObjects;
            this.Textures = Textures;
            this.Meshes = Meshes;
            this.Animations = Animations;
            this.AudioClips = AudioClips;
            this.Shaders = Shaders;
            this.Fonts = Fonts;
            this.VideoClips = VideoClips;
            this.Sprites = Sprites;
            this.ParticleSystems = ParticleSystems;
            this.NavMeshData = NavMeshData;
            this.LightingData = LightingData;
            this.PhysicsMaterials = PhysicsMaterials;
            this.Terrains = Terrains;
            this.RenderTextures = RenderTextures;
        }

        public bool AnyFlagSet()
        {
            return Prefabs || Materials || ScriptableObjects || Textures ||
                   Meshes || Animations || AudioClips || Shaders || Fonts ||
                   VideoClips || Sprites || ParticleSystems || NavMeshData ||
                   LightingData || PhysicsMaterials || Terrains || RenderTextures;
        }
    }
}
