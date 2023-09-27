using System;
using System.Collections;
using System.Collections.Generic;
using Oikos.GameLogic.Systems;
using TMPro;
using UnityEngine;

namespace Oikos.GameLogic.UI {

    public class TrashPickingGameUI : MonoBehaviour {

        #region Attributes

        [Header("Trash Picking Game UI")] 
        [SerializeField, Tooltip("This text is used to display the number of trash objects already picked up.\nThe text will be showed as '0/6 Trash picked up'. or something like that")] private TextMeshProUGUI pickedTrashnumberText = null;

        #endregion

        #region MonoBehaviour's methods

        private void OnEnable() {
            TrashObjectManagerSystem.OnTrashobjectPickedup += UpdateTextSate;
            UpdateTextSate();
        }
        
        private void OnDisable() {
            TrashObjectManagerSystem.OnTrashobjectPickedup -= UpdateTextSate;
        }

        #endregion
        
        #region TrashPickingGameUI's methods

        private void UpdateTextSate() {
            pickedTrashnumberText.text = $"{TrashObjectManagerSystem.PickedUpTrashObjects}/{TrashObjectManagerSystem.NumberOfTrashObjectsSpawned}";
        }

        #endregion

    }
    
}