using System;
using System.Collections.Generic;
using System.Linq;
using Oikos.Core;
using Oikos.Core.SceneManagement;
using Oikos.Core.Systems;
using Oikos.Data;
using Oikos.GameLogic.Props.Spawners;
using Object = UnityEngine.Object;

namespace Oikos.GameLogic.Systems {
    
    public class TrashObjectManagerSystem : AGameSystem {

        #region Properties

        /// <summary>
        /// The list of loaded Trash Objects on the current scene
        /// </summary>
        private static List<TrashObjectData> spawnedTrashObjects = new List<TrashObjectData>();
        
        /// <summary>
        /// Array of every trash objects spawn points
        /// </summary>
        private static TrashObjectSpawnerPoint[] trashSpawnerPoints = null;

        #endregion
        
        #region AGameSystem's virtual methods

        public override void InitializeSystem() {
            InternalName = $"TrashObjectLevelSystem";
            
            IsInitialized = true;
        }

        public override void ReloadSystem() {
            
        }

        public override void DisposeSystem() {
            
        }

        #endregion

        #region TrashObjectManagerSystem's methods

        /// <summary>
        /// Load the related gameplay content on the current scene
        /// </summary>
        private void LoadGameplayContent() {
            Logger.Trace("TrashObjectManager System", "Loading gameplay context...");
            
            //Find every spawners on the current scene
            trashSpawnerPoints = Object.FindObjectsOfType<TrashObjectSpawnerPoint>();
            
            //If there's no Trash Object Spawn Point found, don't execute anything
            if(trashSpawnerPoints == null) {
                Logger.Trace("TrashObjectManager System", $"Gameplay context related content couldn't be loaded! (No TrashObjectSpawnPoint found in the active scene's SceneGameplayData file!)");
                return;
            }
            
            //If there's no Trash Objets to spawn, don't execute anything
            if(SceneManager.ActiveScene == null || SceneManager.ActiveScene?.TrashObjects == null) {
                Logger.Trace("TrashObjectManager System", $"Gameplay context related content couldn't be loaded! (No TrashObjectData files referenced in the active scene's SceneGameplayData file!)");
                return;
            }
            
            //Create a new list of TrashObjectData to copy the data from the SceneGameplayData file
            List<TrashObjectData> _sceneObjects = new List<TrashObjectData>();

            for(int i=0; i<SceneManager.ActiveScene.TrashObjects.Length; i++) { //Populate the new TrashObjectData list
                _sceneObjects.Insert(i, SceneManager.ActiveScene.TrashObjects[i]); //Insert this new element
            }
            
            for(int i=0; i<trashSpawnerPoints.Length; i++) {
                bool _instantiateResult = trashSpawnerPoints[i].InstantiateTrashobject(_sceneObjects.First()); //Spawn the trash object (With the first element of this list)
                if (!_instantiateResult) continue; 
                
                //If this element was correctly spawned, remove this element
                spawnedTrashObjects.Add(_sceneObjects.First()); //Add this object to the loaded objects
                _sceneObjects.Remove(_sceneObjects.First());
            }
            
            //If there's trash objects remaining to spawn, and this scene is allowed to reuse spawn points
            if(_sceneObjects.Count > 0 && SceneManager.ActiveScene.AllowTrashObjectSpawnsReuse) {
                for(int i=0; i<trashSpawnerPoints.Length; i++) { //Re-execute the same action, but with the override flag on the spawn point
                    bool _instantiateResult = trashSpawnerPoints[i].InstantiateTrashobject(_sceneObjects.First(), true); //Spawn the trash object (With the first element of this list)
                    if (!_instantiateResult) continue;
                    
                    //If this element was correctly spawned, remove this element
                    spawnedTrashObjects.Add(_sceneObjects.First()); //Add this object to the loaded objects
                    _sceneObjects.Remove(_sceneObjects.First());
                }
            }
            
            Logger.Trace("TrashObjectManager System", $"Gameplay context related content loaded. ('{_sceneObjects.Count}' Trash Objects didn't spawned)");
        }

        #endregion
        
    }
    
}