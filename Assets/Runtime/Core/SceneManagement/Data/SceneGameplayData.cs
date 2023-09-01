using Oikos.Data;
using Oikos.Types;
using UnityEngine;

namespace Oikos.Core.SceneManagement {
    
    /// <summary>
    /// Contain the Gameplay information data of a scene.
    /// This content shouldn't be modified at runtime.
    /// </summary>
    public class SceneGameplayData : ScriptableObject {

        #region Attributes

        [Header("Scene primitive references")]
        [SerializeField, Tooltip("The SceneReference of this scene")] private SceneReference scene = null;
        [SerializeField, Tooltip("The identifier of this scene.\nThis value can be used to load this scene.")] private E_SCENE_IDENTIFIER sceneIdentifier = E_SCENE_IDENTIFIER.PLACEHOLDER;
        [SerializeField, Tooltip("The identifier string of this scene.\nThis value can be used to this scene.")] private string sceneIdentifierString = string.Empty;
        
        [Header("Scene gameplay references")]
        [SerializeField, Tooltip("The list of every trash objects to spawn at the launch of this scene.")] private TrashObjectData[] trashObjects = null;
        [SerializeField, Tooltip("In the case where there's more trash objects specified than spawn points available for them, if set to true, the spawn points will be reused to spawn the remaining trash objects to spawn.")] private bool allowSpawnReuse = false;
        
        #endregion

        #region Properties

        /// <summary>
        /// The SceneReference of this scene.
        /// </summary>
        public SceneReference Scene { get { return scene; } }
        /// <summary>
        /// The identifier of this scene. Defined by the "E_SCENE_IDENTIFIER" Enum.
        /// </summary>
        public E_SCENE_IDENTIFIER Identifier { get { return sceneIdentifier; } }
        /// <summary>
        /// The string identifier of this scene
        /// </summary>
        public string IdentifierString { get { return sceneIdentifierString; } }
        
        /// <summary>
        /// Array of every trash objects to spawn at the launch of this scene.
        /// </summary>
        public TrashObjectData[] TrashObjects { get { return trashObjects; } }
        /// <summary>
        /// In the case where there's more trash objects specified than spawn points available for them,
        /// if set to true, the spawn points will be reused to spawn the remaining trash objects to spawn.
        /// </summary>
        public bool AllowTrashObjectSpawnsReuse { get { return allowSpawnReuse; } }
        
        #endregion
        
    }
    
}