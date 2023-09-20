using System;
using System.Collections.Generic;
using Oikos.Core.SceneManagement;
using Oikos.GameLogic.Systems;
using UnityEngine;

namespace Oikos.Core.Systems {
    
    public static class GameSystemModule {

        #region Attributes

        /// <summary>
        /// The list of every GameSystem currently loaded
        /// </summary>
        private static Dictionary<E_GAME_SYSTEM_TYPE, AGameSystem> loadedGameSystem = new Dictionary<E_GAME_SYSTEM_TYPE, AGameSystem>();

        #endregion

        #region GameSystemModule's init methods

        /// <summary>
        /// Initialize the GameSystemModule
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] private static void Initialize() {
            SceneManager.OnSceneLoaded += OnSceneChanged;
        }

        /// <summary>
        /// Triggered when a scene is changed.
        /// Used to reload/load GameSystems
        /// </summary>
        private static void OnSceneChanged(SceneGameplayData _newSceneGameplayData) {
            if(_newSceneGameplayData == null) return; //If this scene has no SceneGameplayData, don't execute anything
            
            Logger.Trace("GameSystemModule", "Loading the Game Systems of the current scene...");
            
            E_GAME_SYSTEM_TYPE[] _newSceneSystems = _newSceneGameplayData.GameSystemDefinition.RequiredSystems; //Get the list of GameSystems to execute
            
            if(_newSceneGameplayData.GameSystemDefinition.KeepAlive) { //If this scene wants to keep the current systems alive, reload them
                ReloadAllSystems();
            } else { //Else, shutdown every systems
                ShutdownAllSystems();
            }
            
            //Add the new Systems
            
            for(int i=0; i<_newSceneSystems.Length; i++) {
                if(!IsSystemLaunched(_newSceneSystems[i])) { //If this system isn't already instantiated, do it
                    Type _gameSysType = GameSystemRegister.FindGameSystemFromType(_newSceneSystems[i]);
                    AGameSystem _newSystemInstance = (AGameSystem)Activator.CreateInstance(_gameSysType); //Instantiate the new system
                    loadedGameSystem.Add(_newSceneSystems[i], _newSystemInstance); //Add it into the Systems list
                    _newSystemInstance.InitializeSystem(); //Initialize the system
                }
            }
            
            Logger.Trace("GameSystemModule", $"Loaded '{loadedGameSystem.Count}' Game Systems.");
        }
        
        #endregion

        #region GameSystemModule's methods

        /// <summary>
        /// Shutdown all systems currently running
        /// </summary>
        private static void ShutdownAllSystems() {
            foreach (KeyValuePair<E_GAME_SYSTEM_TYPE, AGameSystem> _currentGameSystem in loadedGameSystem) {
                _currentGameSystem.Value.DisposeSystem(); //Shutdown this system
            }
            
            loadedGameSystem.Clear(); //Clear the list
        }
        
        /// <summary>
        /// Reload all systems currently running
        /// </summary>
        private static void ReloadAllSystems() {
            foreach (KeyValuePair<E_GAME_SYSTEM_TYPE, AGameSystem> _currentGameSystem in loadedGameSystem) {
                _currentGameSystem.Value.ReloadSystem(); //Reload this system
            }
        }

        /// <summary>
        /// Return true if the system asked is launched
        /// </summary>
        public static bool IsSystemLaunched(E_GAME_SYSTEM_TYPE _systemType) {
            foreach (KeyValuePair<E_GAME_SYSTEM_TYPE, AGameSystem> _currentGameSystem in loadedGameSystem) {
                if(_currentGameSystem.Key == _systemType) {
                    return true;
                }
            }
            
            return false;
        }
        
        #endregion
        
        
    }
    
}