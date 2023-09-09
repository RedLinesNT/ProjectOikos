using System;
using System.Linq;
using UnityEngine;

namespace Oikos.Core.SceneManagement {
    
    public static class SceneManager {

        #region Attributes
        
        /// <summary>
        /// Array of every Scene Gameplay Data files.
        /// </summary>
        private static SceneGameplayData[] sceneDatas = null;

        #endregion

        #region Events

        /// <summary>
        /// This event is triggered when a scene has been loaded
        /// </summary>
        private static Action<SceneGameplayData> onSceneLoaded;

        #endregion
        
        #region Properties

        /// <summary>
        /// Triggered when a scene has been loaded
        /// </summary>
        public static event Action<SceneGameplayData> OnSceneLoaded { add { onSceneLoaded += value; } remove { onSceneLoaded -= value; } }
        
        /// <summary>
        /// The active Gameplay Scene.
        /// This value might be null if the current scene loaded doesn't have a SceneGameplayData file made.
        /// </summary>
        public static SceneGameplayData ActiveScene { get; private set; } = null;
        
        /// <summary>
        /// The previous active Gameplay Scene.
        /// This value might be null if the previous scene loaded didn't had a SceneGameplayData file made.
        /// </summary>
        public static SceneGameplayData PreviousActiveScene { get; private set; } = null;
        
        #endregion

        #region SceneManager's loading methods

        /// <summary>
        /// Initialize the SceneManager
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)] private static void Initialize() {
            sceneDatas = ResourceFetcher.GetResourceFilesFromType<SceneGameplayData>(); //Get every SceneGameplayData files
            
            if(sceneDatas == null || sceneDatas.Length <= 0) { //If no SceneGameplayData files has been found
                Logger.TraceWarning("Scene Manager", "There's no SceneGameplayData files found! This scene manager will no longer allow calls.");
                return;
            }
            
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (_scene, _loadMode) => {
                PreviousActiveScene = ActiveScene;
                ActiveScene = FindSceneGameplayDataFromPath(_scene.path);
                onSceneLoaded?.Invoke(ActiveScene);
            };
        }

        #endregion

        #region SceneManager's Getters

        /// <summary>
        /// Tries to find a SceneGameplayData file from a Scene ID (E_SCENE_IDENTIFIER)
        /// </summary>
        public static SceneGameplayData FindSceneGameplayDataFromID(E_SCENE_IDENTIFIER _targetSceneIdentifier) {
            return sceneDatas.FirstOrDefault(_t => _t.Identifier == _targetSceneIdentifier);
        }
        
        /// <summary>
        /// Tries to find a SceneGameplayData file from a scene's path
        /// </summary>
        public static SceneGameplayData FindSceneGameplayDataFromPath(string _targetScenePath) {
            return sceneDatas.FirstOrDefault(_t => _t.Scene.ScenePath == _targetScenePath);
        }
        
        /// <summary>
        /// Tries to find a SceneGameplayData file from the scene's string identifier
        /// </summary>
        public static SceneGameplayData FindSceneGameplayDataFromStringID(string _stringIdentifier) {
            return sceneDatas.FirstOrDefault(_t => _t.IdentifierString == _stringIdentifier);
        }

        #endregion
        
    }

    
}