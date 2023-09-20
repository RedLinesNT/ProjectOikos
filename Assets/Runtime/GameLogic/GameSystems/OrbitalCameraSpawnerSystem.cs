using System.Collections;
using System.Collections.Generic;
using Oikos.Core.Systems;
using Oikos.GameLogic.Controller;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Logger = Oikos.Core.Logger;

namespace Oikos.GameLogic.Systems {
    
    public class OrbitalCameraSpawnerSystem : AGameSystem {

        #region Atrtibutes

        /// <summary>
        /// The Addressable link of the OrbitalCameraController prefab
        /// </summary>
        private static readonly string cameraControllerAddressableKeyString = "Assets/Prefabs/Controllers/OrbitalCameraController.prefab";

        #endregion

        #region Properties

        /// <summary>
        /// The Orbital Camera Controller spawned
        /// </summary>
        public static OrbitalCameraController OrbitalCameraController { get; private set; } = null;

        #endregion

        #region AGameSystem's virtual methods

        public override void InitializeSystem() {
            InternalName = $"OrbitalCameraSpawnerSystem";
            
            SpawnCameraController();
            
            IsInitialized = true;
        }

        public override void ReloadSystem() {
            
        }

        public override void DisposeSystem() {
            
        }

        #endregion

        #region OrbitalCameraSpawnerSystem's methods

        /// <summary>
        /// Spawn the OrbitalCameraController
        /// </summary>
        private void SpawnCameraController() {
            AsyncOperationHandle<GameObject> _prefabHandle = Addressables.LoadAssetAsync<GameObject>(cameraControllerAddressableKeyString);

            _prefabHandle.Completed += _operationResult => {
                if(_prefabHandle.Status == AsyncOperationStatus.Succeeded) { //If the operation was a success
                    GameObject _prefabResult = _operationResult.Result; 
                    GameObject _prefabInstance = Object.Instantiate(_prefabResult); //Instantiate the prefab
                
                    OrbitalCameraController = _prefabInstance.GetComponent<OrbitalCameraController>();
                } else { //If the operation wasn't a success
                    Logger.TraceError("OrbitalCameraSpawner System", $"Failed to fetch the OrbitalCameraController prefab from Addressable link!");
                }
            };
            
            
            
            Addressables.Release(_prefabHandle); //Release the operation
        }

        #endregion
        
    }
    
}