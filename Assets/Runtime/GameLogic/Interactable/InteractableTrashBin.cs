using System.Collections;
using System.Collections.Generic;
using Oikos.Data;
using UnityEngine;

namespace Oikos.GameLogic.Interactable {

    public class InteractableTrashBin : APointerClickableObject {

        #region Attributes

        [Header("Interactable Trash Bin - References")]
        [SerializeField, Tooltip("The TrashBinData file of this trash bin.")] private TrashBinData dataFile = null;

        #endregion

        #region Properties

        /// <summary>
        /// The TrashBinData file of this Trash bin.
        /// </summary>
        public TrashBinData DataFile { get { return dataFile; } }

        #endregion
        
        #region APointerClickableObject's virtual methods

        private protected override void OnPointerClick() {
            //TODO: Implement code logic here?
        }

        private protected override void OnPointerEnter() {
            //TODO: Implement code logic here?
        }

        private protected override void OnPointerExit() {
            //TODO: Implement code logic here?
        }

        #endregion
        
    }
    
}