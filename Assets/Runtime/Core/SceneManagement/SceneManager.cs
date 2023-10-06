using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Timers;
using Oikos.Core.UI;
using UnityEngine;
using Timer = System.Timers.Timer;

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
                
                UIWidgetSystem.DisableUIWidget(E_UI_WIDGET_TYPE.LOADING_SCREEN);
            };
        }

        #endregion

        public static void FakeSceneLoad(string _sceneName, float _fakeTime) {
            UIWidgetSystem.EnableUIWidget(E_UI_WIDGET_TYPE.LOADING_SCREEN);

            Logger.Trace("Timer", "Je prepare le timer");
            
            Timer fakeTimer = new Timer();
            fakeTimer.Elapsed += (sender, args) => { FakeSceneLoadInternal(_sceneName); };
            fakeTimer.Interval = 5000f; //5 Seconds
            fakeTimer.AutoReset = false;
            fakeTimer.Start();

            Logger.Trace("Timer", "Je lance le timer");
        }

        private static void FakeSceneLoadInternal(string _sceneName) {
            Logger.Trace("Timer", "Le timer a terminé d'attendre 5000MS");

            UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
        }

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