using UnityEngine;
using Logger = Oikos.Core.Logger;

namespace Oikos.GameLogic.Interactable {
    
    public class InteractableTrashobject : APointerClickableObject {

        #region Attributes

        [Header("Interactable Trash Object - References")]
        [SerializeField, Tooltip("The rigidbody component of this trash object prefab")] private Rigidbody rigidbodyComponent = null;
        
        #endregion

        #region Properties

        /// <summary>
        /// The Rigidbody component of this trash object
        /// </summary>
        public Rigidbody RigidbodyComponent { get { return rigidbodyComponent; } }

        #endregion
        
        private protected override void OnPointerClick() {
            //TODO: Implement code logic here?
        }

        private protected override void OnPointerEnter() {
            //TODO: Implement code logic here?
        }

        private protected override void OnPointerExit() {
            //TODO: Implement code logic here?
        }
    }
    
}