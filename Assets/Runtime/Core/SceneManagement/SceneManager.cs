using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Oikos.Data;
using Oikos.GameLogic.Props.Spawners;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Oikos.Core.SceneManagement {
    
    public static class SceneManager {

        #region Attributes

        /// <summary>
        /// Is the SceneManager able to be used
        /// </summary>
        private static bool IsInitialized = false;
        
        /// <summary>
        /// Array of every Scene Gameplay Data files.
        /// </summary>
        private static SceneGameplayData[] sceneDatas = null;

        /// <summary>
        /// Array of every trash objects spawn points
        /// </summary>
        private static TrashObjectSpawnerPoint[] trashSpawnerPoints = null;
        
        #endregion

        #region Properties

        /// <summary>
        /// The active Gameplay Scene.
        /// This value might be null if the current scene loaded doesn't have a SceneGameplayData file made.
        /// </summary>
        public static SceneGameplayData ActiveScene { get; private set; } = null;
        
        /// <summary>
        /// The list of loaded Trash Objects on the current scene
        /// </summary>
        public static List<TrashObjectData> SpawnedTrashObjects { get; private set; } = new List<TrashObjectData>();

        #endregion

        #region SceneManager's loading methods

        /// <summary>
        /// Initialize the SceneManager
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] private static void Initialize() {
            sceneDatas = ResourceFetcher.GetResourceFilesFromType<SceneGameplayData>(); //Get every SceneGameplayData files
            
            if(sceneDatas == null) { //If no SceneGameplayData files has been found
                Logger.TraceWarning("Scene Manager", "There's no SceneGameplayData files found! This scene manager will no longer allow calls.");
                return;
            }
            
            IsInitialized = true;
        }

        /// <summary>
        /// The method is only used for the first scene loaded. Used to still load the required content.
        /// Useful in the editor.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)] private static void PreloadFirstSceneContent() {
            ActiveScene = FindSceneGameplayDataFromPath(UnityEngine.SceneManagement.SceneManager.GetActiveScene().path); //Try to get the config file from the current scene laoded
            if(ActiveScene != null) { //Load the gameplay context if the Scene config file has been found
                LoadSceneGameplayContext(ActiveScene);
            } else {
                Logger.Trace("Scene Manager", $"The scene used at the game launch don't have any SceneGameplayData linked! (Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name})");
            }
        }
        
        /// <summary>
        /// Load the gameplay elements of a scene.
        /// </summary>
        private static void LoadSceneGameplayContext(SceneGameplayData _sceneGameplay, Action _onActionFinished = null) {
            if(!IsInitialized) return; //Don't execute anything if not allowed to do so

            Logger.Trace("Scene Manager", "Loading gameplay context...");
            
            trashSpawnerPoints = Object.FindObjectsOfType<TrashObjectSpawnerPoint>(); //Get every spawner points
            
            //If there's no Trash Object Spawn Point found, don't execute anything
            if(trashSpawnerPoints == null) {
                Logger.Trace("Scene Manager", $"Gameplay context related content couldn't be loaded! (No TrashObjectSpawnPoint found in the active scene!)");
                _onActionFinished?.Invoke(); //Trigger the finished event
                return;
            }
            
            //If there's no Trash Objects to spawn, don't execute anything
            if(_sceneGameplay.TrashObjects == null) {
                Logger.Trace("Scene Manager", $"Gameplay context related content couldn't be loaded! (No TrashObjectData files referenced in the active scene's SceneGameplayData file!)");
                _onActionFinished?.Invoke(); //Trigger the finished event
                return;
            }
            
            List<TrashObjectData> _sceneObjects = new List<TrashObjectData>(); //Create a new list of TrashObjectData to copy the data from the SceneGameplayData file
            
            for(int i=0; i<_sceneGameplay.TrashObjects.Length; i++) { //Populate the new TrashObjectData list
                _sceneObjects.Insert(i, _sceneGameplay.TrashObjects[i]); //Insert this new element
            }
            
            for(int i=0; i<trashSpawnerPoints.Length; i++) {
                bool _instantiateResult = trashSpawnerPoints[i].InstantiateTrashobject(_sceneObjects.First()); //Spawn the trash object (With the first element of this list)
                if(_instantiateResult) { //If this element was correctly spawned, remove this element
                    SpawnedTrashObjects.Add(_sceneObjects.First()); //Add this object to the loaded objects
                    _sceneObjects.Remove(_sceneObjects.First());
                }
            }
            
            //If there's trash objects list to spawn, and this scene is allowed to reuse spawn points
            if(_sceneObjects.Count > 0 && _sceneGameplay.AllowTrashObjectSpawnsReuse) {
                for(int i=0; i<trashSpawnerPoints.Length; i++) { //Re-execute the same action, but with the override flag on the spawn point
                    bool _instantiateResult = trashSpawnerPoints[i].InstantiateTrashobject(_sceneObjects.First(), true); //Spawn the trash object (With the first element of this list)
                    if(_instantiateResult) { //If this element was correctly spawned, remove this element
                        SpawnedTrashObjects.Add(_sceneObjects.First()); //Add this object to the loaded objects
                        _sceneObjects.Remove(_sceneObjects.First());
                    }
                }
            }
            
            Logger.Trace("Scene Manager", $"Gameplay context related content loaded. ('{_sceneObjects.Count}' Trash Objects didn't spawned)");
            
            //Trigger the finished event
            _onActionFinished?.Invoke();
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