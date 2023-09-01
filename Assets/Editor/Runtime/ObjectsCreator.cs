using System.IO;
using Oikos.Core.SceneManagement;
using Oikos.Data;
using UnityEditor;
using UnityEngine;

namespace Oikos.Editor {
    
    /// <summary>
    /// Creates ScriptableObjects instances as files inside the Editor
    /// </summary>
    public static class ObjectsCreator {
        
        /// <summary>
        /// Create a TrashObjectData (ScriptableObject) file
        /// </summary>
        [MenuItem("Oikos/Gameplay/Trash Object file")] public static void CreateTrashObjectFile() {
            TrashObjectData _asset = ScriptableObject.CreateInstance<TrashObjectData>();
            
            if(!Directory.Exists("Assets/Resources/Gameplay/Trash Objects/")) { //Check if the directory exists
                Directory.CreateDirectory("Assets/Resources/Gameplay/Trash Objects/");
            }
            
            string _assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Gameplay/Trash Objects/New Object.asset");
            AssetDatabase.CreateAsset(_asset, _assetPath);
            AssetDatabase.SaveAssets();
            
            EditorUtility.FocusProjectWindow();
            
            Selection.activeObject = _asset;
        }
        
        /// <summary>
        /// Create a SceneGameplayData (ScriptableObject) file
        /// </summary>
        [MenuItem("Oikos/Gameplay/Scene Gameplay file")] public static void CreateSceneGameplayFile() {
            SceneGameplayData _asset = ScriptableObject.CreateInstance<SceneGameplayData>();
            
            if(!Directory.Exists("Assets/Resources/Scene Data/")) { //Check if the directory exists
                Directory.CreateDirectory("Assets/Resources/Scene Data/");
            }
            
            string _assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Scene Data/New Scene Gameplay Data.asset");
            AssetDatabase.CreateAsset(_asset, _assetPath);
            AssetDatabase.SaveAssets();
            
            EditorUtility.FocusProjectWindow();
            
            Selection.activeObject = _asset;
        } 
        
    }
    
}