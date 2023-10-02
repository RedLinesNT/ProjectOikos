using System;
using System.Collections.Generic;
using System.Linq;
using Oikos.Core.SceneManagement;
using Oikos.Core.Systems;
using Oikos.Core.UI;
using Oikos.Data;
using Oikos.GameLogic.Interactable;
using Oikos.GameLogic.Props.Spawners;
using UnityEngine;
using Logger = Oikos.Core.Logger;
using Object = UnityEngine.Object;

namespace Oikos.GameLogic.Systems {
    
    public class TrashObjectManagerSystem : AGameSystem {

        #region Attributes

        /// <summary>
        /// Array of every trash objects spawn points
        /// </summary>
        private TrashObjectSpawnerPoint[] trashSpawnerPoints = null;

        #endregion

        #region Properties

        /// <summary>
        /// The TrashObjectData file of the last Trash Interactable Object hit
        /// </summary>
        public static TrashObjectData LastTrashObjectHit { get; private set; }
        
        /// <summary>
        /// The TrashObject's Spawn point of the list Trash Object hit
        /// </summary>
        public static InteractableTrashobject LastTrashObjectHitInteractablePoint { get; private set; }

        /// <summary>
        /// The number of spawned Trash Objects on the scene.
        /// </summary>
        public static int NumberOfTrashObjectsSpawned { get; private set; } = 0;

        /// <summary>
        /// The number of Trash Objects on the scene that have been picked up by the player.
        /// </summary>
        public static int PickedUpTrashObjects { get; private set; } = 0;
        
        /// <summary>
        /// Triggered when the player pickup a Trash Object set on the current scene.
        /// </summary>
        public static event Action OnTrashobjectPickedup { add { onObjectPickedUp += value; } remove { onObjectPickedUp -= value; } }
        
        #endregion

        #region Events

        /// <summary>
        /// Triggered when the player picked up a trash object set on the current scene.
        /// </summary>
        private static Action onObjectPickedUp;

        #endregion
        
        #region AGameSystem's virtual methods

        public override void InitializeSystem() {
            InternalName = $"TrashObjectLevelSystem";

            NumberOfTrashObjectsSpawned = 0;
            PickedUpTrashObjects = 0;
            LastTrashObjectHit = null;
            
            LoadGameplayContent();
            
            IsInitialized = true;
        }

        public override void ReloadSystem() {
            NumberOfTrashObjectsSpawned = 0;
            PickedUpTrashObjects = 0;
            LastTrashObjectHit = null;
            
            LoadGameplayContent();
        }

        public override void DisposeSystem() {
            NumberOfTrashObjectsSpawned = 0;
            PickedUpTrashObjects = 0;
            LastTrashObjectHit = null;
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
            
            //TODO: You better Kill yourself NOW!!! (#Unity2023 <3)
            //Now try to spawn the trash objects into the scene
            /*for(int i=0; i<_sceneObjectsSpawnPoints.Count; i++) {
                if(_sceneObjects.Count <= 0) { //There's no items left to spawn
                    break; //Break on this loop
                }
                
                if(_sceneObjectsSpawnPoints[i].UseWantedObject) { //If this spawn point only want a specific trash object
                    for(int y=0; y<_sceneObjects.Count; y++) { //Find if the Scene trash objects definition contain the wanted item
                        if(_sceneObjectsSpawnPoints[i].WantedObject == _sceneObjects[y].Identifier) { //If the object's type is the one asked, Instantiate it
                            InteractableTrashobject _newTrashObjectConstraint = _sceneObjectsSpawnPoints[i].InstantiateTrashObject(_sceneObjects[y]);
                            _newTrashObjectConstraint.IsInteractable = true;
                            _newTrashObjectConstraint.TrashObjectData = _sceneObjects[y];
                            _newTrashObjectConstraint.OnPointerClickEvent += () => { OnTrashObjectPickedUp(_newTrashObjectConstraint); }; //Bind the event
                            
                            //Remove this spawn point and item from the lists
                            _sceneObjectsSpawnPoints.RemoveAt(i); //Remove the spawn point
                            _sceneObjects.RemoveAt(y); //Remove the TrashObject
                            
                            if(_sceneObjects.Count <= 0) { //There's no items left to spawn
                                _sceneObjects.Sort();
                            }
                            
                            break; //Break on the item's loop
                        }
                    }
                } else {
                    //Just spawn the next item on this spawn point
                    InteractableTrashobject _newTrashObject = _sceneObjectsSpawnPoints[i].InstantiateTrashObject(_sceneObjects.First());
                    _newTrashObject.IsInteractable = true;
                    _newTrashObject.TrashObjectData = _sceneObjects.First();
                    _newTrashObject.OnPointerClickEvent += () => { OnTrashObjectPickedUp(_newTrashObject); };
                
                    //Remove this spawn point and item from the lists
                    _sceneObjectsSpawnPoints.RemoveAt(i); //Remove the spawn point
                    _sceneObjects.Remove(_sceneObjects.First()); //Remove the TrashObject
                    
                    if(_sceneObjects.Count <= 0) { //There's no items left to spawn
                        _sceneObjects.Sort();
                    }
                }
            }*/
            
            for(int i=0; i<_sceneObjects.Count; i++) {
                if(_sceneObjects.Count <= 0) { //There's no items left to spawn
                    break; //Break on this loop
                }
                
                //Just spawn the next item on this spawn point
                InteractableTrashobject _newTrashObject = _sceneObjectsSpawnPoints.First().InstantiateTrashObject(_sceneObjects[i]);
                
                if(_newTrashObject == null) break;
                
                _newTrashObject.IsInteractable = true;
                _newTrashObject.TrashObjectData = _sceneObjects[i];
                _newTrashObject.OnPointerClickEvent += () => { OnTrashObjectPickedUp(_newTrashObject); };
                
                //Remove this spawn point and item from the lists
                _sceneObjectsSpawnPoints.Remove(_sceneObjectsSpawnPoints.First()); //Remove the spawn point

                NumberOfTrashObjectsSpawned++;
            }
            
            //Enable the UI
            UIWidgetSystem.EnableUIWidget(E_UI_WIDGET_TYPE.TRASH_GAMEPLAY_UI_STATE);
            
            Logger.Trace("TrashObjectManager System", $"Gameplay context related content loaded.");
        }

        /// <summary>
        /// Triggered when a TrashObject on the scene has been clicked on.
        /// </summary>
        /// <param name="_objectClicked">The object that has been clicked on</param>
        private void OnTrashObjectPickedUp(InteractableTrashobject _objectClicked) {
            LastTrashObjectHit = _objectClicked.TrashObjectData;
            LastTrashObjectHitInteractablePoint = _objectClicked;
            PickedUpTrashObjects++;

            if (GameSystemModule.IsSystemLaunched(E_GAME_SYSTEM_TYPE.ORBITAL_CAMERA_CONTROLLER_SPAWNER) && OrbitalCameraSpawnerSystem.OrbitalCameraController != null) {
                OrbitalCameraSpawnerSystem.OrbitalCameraController.MoveTrashObjectToViewPoint(_objectClicked, 1f);
                OrbitalCameraSpawnerSystem.OrbitalCameraController.ZoomMax();
            }
            
            //Trigger the event
            onObjectPickedUp?.Invoke();
            
            //Disable the UI
            UIWidgetSystem.DisableUIWidget(E_UI_WIDGET_TYPE.TRASH_GAMEPLAY_UI_STATE);
            
            UIWidgetSystem.EnableUIWidget(E_UI_WIDGET_TYPE.TRASH_OBJECT_WORLD_IMPACT_DESC_SCREEN);

            
        }

        public static void OnPickupScreenInfoDismissed() {
            if (GameSystemModule.IsSystemLaunched(E_GAME_SYSTEM_TYPE.ORBITAL_CAMERA_CONTROLLER_SPAWNER) && OrbitalCameraSpawnerSystem.OrbitalCameraController != null) {
                OrbitalCameraSpawnerSystem.OrbitalCameraController.MoveTrashObjectOutOfCameraView(LastTrashObjectHitInteractablePoint, 1f, () => MonoBehaviour.Destroy(LastTrashObjectHitInteractablePoint.gameObject));
            }
            
            //Enable the UI
            UIWidgetSystem.EnableUIWidget(E_UI_WIDGET_TYPE.TRASH_GAMEPLAY_UI_STATE);
            
            UIWidgetSystem.DisableUIWidget(E_UI_WIDGET_TYPE.TRASH_OBJECT_WORLD_IMPACT_DESC_SCREEN);
        }
        
        #endregion
        
    }
    
}