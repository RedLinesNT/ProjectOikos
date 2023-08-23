using UnityEngine;

namespace Oikos.Core.SceneManagement {
    
    /// <summary>
    /// Contain the Information Data of a Scene.
    /// This content shouldn't be modified at runtime.
    /// </summary>
    public class SceneInfoData : ScriptableObject {

        #region Attributes

        [Header("Scene basic references")]
        [SerializeField, Tooltip("The SceneReference of this scene.")] private SceneReference scene = null;
        [SerializeField, Tooltip("The identifier of this scene. \nThis value can be used to load it for example.")] private E_SCENE_IDENTIFIER sceneIdentifier = E_SCENE_IDENTIFIER.UNKNOWN;
        [SerializeField, Tooltip("The string identifier of this scene. \nThis value can be used to load it for example.")] private string sceneIdentifierString = string.Empty;

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
        /// The string identifier of this scene.
        /// </summary>
        public string IdentifierString { get { return sceneIdentifierString; } }
        
        #endregion
        
    }
    
}