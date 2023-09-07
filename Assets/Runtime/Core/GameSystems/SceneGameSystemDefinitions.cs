using System;
using UnityEngine;

namespace Oikos.Core.Systems {
    
    [Serializable] public class SceneGameSystemDefinition {

        #region Attributes

        [Header("Scene Game Systems Definition")]
        [SerializeField, Tooltip("If set to true, the Game Systems launch on the previous scene will be kept alive.")] private bool keepAlivePreviousSystems = false;
        [SerializeField, Tooltip("The Game Systems to launch on this scene")] private E_GAME_SYSTEM_TYPE[] requiredSystems = null;
        
        #endregion

        #region Properties

        /// <summary>
        /// Should the scene keep alive the systems launched on the previous scene
        /// </summary>
        public bool KeepAlive { get { return keepAlivePreviousSystems; } }

        /// <summary>
        /// The required systems to launch on this scene
        /// </summary>
        public E_GAME_SYSTEM_TYPE[] RequiredSystems { get { return requiredSystems; } }
        
        #endregion
        
    }
    
}