using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oikos.Core {
    
    /// <summary>
    /// This class manages and keeps track of every Camera Entities registered in game.
    /// </summary>
    public static class CameraEntitySystem {

        #region Attributes

        /// <summary>
        /// The list of every Camera Entities registered
        /// </summary>
        private static List<ACameraEntity> camEntities = new List<ACameraEntity>(); 

        #endregion

        #region Events

        /// <summary>
        /// Triggered when a CameraEntity has been registered
        /// </summary>
        private static Action<ACameraEntity> onEntityRegistered;
        
        /// <summary>
        /// Triggered when a CameraEntity has been unregistered
        /// </summary>
        private static Action<ACameraEntity> onEntityUnregistered;

        #endregion

        #region Properties

        /// <summary>
        /// The number of CameraEntities registered
        /// </summary>
        public static int EntitiesRegisteredCount { get { return camEntities.Count; } }

        /// <summary>
        /// Triggered when a CameraEntity has been registered
        /// </summary>
        public static event Action<ACameraEntity> OnEntityRegistered { add { onEntityRegistered += value; } remove { onEntityRegistered -= value; } }
        /// <summary>
        /// Triggered when a CameraEntity has been unregistered
        /// </summary>
        public static event Action<ACameraEntity> OnEntityUnregistered { add { onEntityUnregistered += value; } remove { onEntityUnregistered -= value; } }

        #endregion
        
        #region CameraEntitySystem's Registering Methods

        /// <summary>
        /// Register a new CameraEntity in this Entity System.
        /// </summary>
        /// <description>
        /// If this CameraEntity is already registered, or the type specified for this entity is already used,
        /// this CameraEntity will not be registered but will stay alive on the scene
        /// </description>
        /// <param name="_camEntity">The CameraEntity to register</param>
        public static void RegisterCameraEntity(ACameraEntity _camEntity) {
            bool _isEntityRegistered = GetCameraEntity(_camEntity.CameraIdentifier); //Is this CameraEntity already registered
            bool _isTypeRegistered = GetCameraEntity(_camEntity.CameraType); //Is this type already used
            
            if(_isEntityRegistered) {
                Logger.TraceError("Camera Entity System", $"Unable to register a CameraEntity. This CameraEntity is already registered! This camera will not be destroyed.");
                return;
            }
            
            if(_isTypeRegistered) {
                Logger.TraceError("Camera Entity System", $"Unable to register a CameraEntity. The CameraEntity's '{_camEntity.CameraType}' type is already used! This camera will not be destroyed.");
                return;
            }
            
            camEntities.Add(_camEntity); //Add the CameraEntity to the list
            onEntityRegistered?.Invoke(_camEntity); //Trigger the event
            
            Logger.Trace("Camera Entity System", $"CameraEntity (NAME:'{_camEntity.CameraName}' - ID:'{_camEntity.CameraIdentifier}', TYPE:'{_camEntity.CameraType}') has been registered!");
        }
        
        /// <summary>
        /// Unregister an existing CameraEntity in this Entity System.
        /// </summary>
        /// <param name="_camEntity">The CameraEntity to unregister</param>
        public static void UnregisterCameraEntity(ACameraEntity _camEntity) {
            bool _isEntityRegistered = GetCameraEntity(_camEntity.CameraIdentifier); //Is this CameraEntity registered
            
            if(!_isEntityRegistered) {
                Logger.TraceError("Camera Entity System", $"Unable to unregister a CameraEntity. This CameraEntity isn't registered!");
                return;
            }
            
            camEntities.Remove(_camEntity); //Remove the CameraEntity from the list
            onEntityUnregistered?.Invoke(_camEntity); //Trigger the event
            
            Logger.Trace("Camera Entity System", $"CameraEntity (NAME:'{_camEntity.CameraName}' - ID:'{_camEntity.CameraIdentifier}', TYPE:'{_camEntity.CameraType}') has been unregistered!");
        }

        #endregion
        
        #region CameraEntitySystem's Getters

        /// <summary>
        /// Try to find a registered Camera Entity from its ID
        /// </summary>
        /// <param name="_camIdentifier">The Camera Entity's identifier</param>
        /// <returns>The Camera Entity found. (The result can be null!)</returns>
        public static ACameraEntity GetCameraEntity(string _camIdentifier) {
            return camEntities.Find(_x => _x.CameraIdentifier == _camIdentifier);
        }
        
        /// <summary>
        /// Try to find a registered Camera Entity from its UnityEngine.Camera component
        /// </summary>
        /// <param name="_camera">The Camera Entity's Camera component</param>
        /// <returns>The Camera Entity found. (The result can be null!)</returns>
        public static ACameraEntity GetCameraEntity(Camera _camera) {
            return camEntities.Find(_x => _x.CameraComponent == _camera);
        }
        
        /// <summary>
        /// Try to find a registered Camera Entity from its type.
        /// Defined by the "E_CAMERA_ENTITY_TYPE" Enum.
        /// </summary>
        /// <description>
        /// E_CAMERA_ENTITY_TYPE.UNSPECIFIED will return a null object!
        /// </description>
        /// <param name="_camType">The Camera Entity's type</param>
        /// <returns>The Camera Entity found. (The result can be null!)</returns>
        public static ACameraEntity GetCameraEntity(E_CAMERA_ENTITY_TYPE _camType) {
            if(_camType == E_CAMERA_ENTITY_TYPE.UNSPECIFIED) return null;
            
            return camEntities.Find(_x => _x.CameraType == _camType);
        }

        #endregion
        
    }
    
}