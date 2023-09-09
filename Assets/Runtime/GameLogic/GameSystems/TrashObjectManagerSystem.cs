using System;
using System.Collections.Generic;
using System.Linq;
using Oikos.Core;
using Oikos.Core.SceneManagement;
using Oikos.Core.Systems;
using Oikos.Data;
using Oikos.GameLogic.Interactable;
using Oikos.GameLogic.Props.Spawners;
using Object = UnityEngine.Object;

namespace Oikos.GameLogic.Systems {
    
    public class TrashObjectManagerSystem : AGameSystem {

        #region Properties

        /// <summary>
        /// Array of every trash objects spawn points
        /// </summary>
        private static TrashObjectSpawnerPoint[] trashSpawnerPoints = null;

        #endregion
        
        #region AGameSystem's virtual methods

        public override void InitializeSystem() {
            InternalName = $"TrashObjectLevelSystem";
            
            LoadGameplayContent();
            
            IsInitialized = true;
        }

        public override void ReloadSystem() {
            LoadGameplayContent();
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
                Logger.Trace("TrashObjectManager System", $"Gameplay context related content couldn't be loaded! (No TrashObjectSpawnPoint found in the active scene!)");
                return;
            }
            
            //If there's no Trash Objets to spawn, don't execute anything
            if(SceneManager.ActiveScene == null || SceneManager.ActiveScene?.TrashObjects == null) {
                Logger.Trace("TrashObjectManager System", $"Gameplay context related content couldn't be loaded! (No TrashObjectData files referenced in the active scene's SceneGameplayData file!)");
                return;
            }
            
            List<TrashObjectData> _sceneObjects = new List<TrashObjectData>(); //Create a new list of TrashObjectData to copy the data from the SceneGameplayData file
            List<TrashObjectSpawnerPoint> _sceneObjectsSpawnPoints = new List<TrashObjectSpawnerPoint>(); //Create a new list of TrashObjectSpawnerPoint to copy the Trash Spawners from the current active scene
            
            for(int i=0; i<SceneManager.ActiveScene.TrashObjects.Length; i++) { //Populate the new TrashObjectData list
                _sceneObjects.Insert(i, SceneManager.ActiveScene.TrashObjects[i]); //Insert this new element
            }
            
            for(int i=0; i<trashSpawnerPoints.Length; i++) { //Populate the new TrashObjectSpawnerPoint list
                _sceneObjectsSpawnPoints.Insert(i, trashSpawnerPoints[i]); //Insert this new element
            }
            
            //Now try to spawn the trash objects into the scene
            for(int i=0; i<_sceneObjectsSpawnPoints.Count; i++) {
                if(_sceneObjects.Count <= 0) { //There's no items left to spawn
                    break; //Break on this for loop
                }
                
                if(_sceneObjectsSpawnPoints[i].UseWantedObject) { //If this spawn point only want a specific trash object
                    for(int y=0; y<_sceneObjects.Count; y++) { //Find if the Scene trash objects definition contain the wanted item
                        if(_sceneObjectsSpawnPoints[i].WantedObject == _sceneObjects[y].Identifier) { //If the object's type is the one asked, Instantiate it
                            InteractableTrashobject _newTrashObjectConstraint = _sceneObjectsSpawnPoints[i].InstantiateTrashObject(_sceneObjects[y]);
                            _newTrashObjectConstraint.OnPointerClickEvent += () => { OnTrashObjectPickedUp(_newTrashObjectConstraint); }; //Bind the event
                            
                            //Remove this spawn point and item from the lists
                            _sceneObjectsSpawnPoints.RemoveAt(i); //Remove the spawn point
                            _sceneObjects.RemoveAt(y); //Remove the TrashObject
                            
                            _sceneObjects.Sort();
                            
                            break; //Break on the item's loop
                        }
                    }
                } else {
                    //Just spawn the next item on this spawn point
                    InteractableTrashobject _newTrashObject = _sceneObjectsSpawnPoints[i].InstantiateTrashObject(_sceneObjects.First());
                    _newTrashObject.OnPointerClickEvent += () => { OnTrashObjectPickedUp(_newTrashObject); };
                
                    //Remove this spawn point and item from the lists
                    _sceneObjectsSpawnPoints.RemoveAt(i); //Remove the spawn point
                    _sceneObjects.Remove(_sceneObjects.First()); //Remove the TrashObject
                    
                    _sceneObjects.Sort();
                }
            }
            
            
            Logger.Trace("TrashObjectManager System", $"Gameplay context related content loaded.");
        }

        /// <summary>
        /// Triggered when a TrashObject on the scene has been clicked on.
        /// </summary>
        /// <param name="_objectClicked">The object that has been clicked on</param>
        private void OnTrashObjectPickedUp(InteractableTrashobject _objectClicked) {
            Logger.Trace("TrashObjectManager System", $"Trash object: {_objectClicked.name} has been clicked on");
        }
        
        #endregion
        
    }
    
}