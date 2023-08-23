using System.IO;
using Oikos.Core.SceneManagement;
using UnityEditor;
using UnityEngine;

namespace Oikos.Editor {
    
    public static class CoreSOCreator {
        
        /// <summary>
        /// Create a new SceneInfoData ScriptableObject file
        /// </summary>
        [MenuItem("Oikos/Engine Config/Scene Information Data")]
        public static void CreateSceneInfoDataFile() {
            SceneInfoData _asset = ScriptableObject.CreateInstance<SceneInfoData>();
            
            //Check if the directory exists
            if(!Directory.Exists("Assets/Resources/Engine config/Scene Data/")) {
                Directory.CreateDirectory("Assets/Resources/Engine config/Scene Data/");
            }
            
            string _name = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Engine config/Scene Data/New Scene Info Config.asset");
            AssetDatabase.CreateAsset(_asset, _name);
            AssetDatabase.SaveAssets();
            
            EditorUtility.FocusProjectWindow();
            
            Selection.activeObject = _asset;
        }
        
    }
    
}