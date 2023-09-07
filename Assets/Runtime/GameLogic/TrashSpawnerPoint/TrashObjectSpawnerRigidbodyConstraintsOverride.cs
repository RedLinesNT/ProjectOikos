using UnityEngine;

namespace Oikos.GameLogic.Props.Spawners {
    
    public class TrashObjectSpawnerRigidbodyConstraintsOverride : MonoBehaviour {

        #region Attributes

        [Header("Trash Object Spawner Rigidbody Constraints Overrides")]
        [SerializeField, Tooltip("Enable the item's rigidbody. This will work only if a Rigidbody Component can be found.")] private bool enableRigidbody = false;
        [SerializeField, Tooltip("Set physics constraints of this trash item's rigidbody.\nEnableRigidbody must be set to true.")] private RigidbodyConstraints rigidbodyConstraints = RigidbodyConstraints.None;
        
        #endregion

        #region Properties

        /// <summary>
        /// Enable the item's rigidbody.
        /// This will work only if a Rigidbody Component can be found.
        /// </summary>
        public bool EnableRigidbody { get { return enableRigidbody; } }
        /// <summary>
        /// The Rigidbody's physics constraints.
        /// EnableRigidbody must be set to true in order to be taken in account.
        /// </summary>
        public RigidbodyConstraints RigidbodyConstraints { get { return rigidbodyConstraints; } }

        #endregion
        
    }

    
}