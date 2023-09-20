using System;
using Oikos.Core;
using Oikos.Core.Systems;
using Oikos.Core.UI;
using Oikos.GameLogic.Controller;
using Oikos.GameLogic.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Oikos.GameLogic.UI {

    public class TrashObjectScreenInfo : MonoBehaviour {

        #region Attributes

        [Header("UI References")]
        [SerializeField, Tooltip("The TextMeshPro text component used to display the pickup line of a Trash Object.")] private TextMeshProUGUI pickupLineText = null;
        
        #endregion

        #region MonoBehaviour's methods

        private void OnEnable() {
            if (GameSystemModule.IsSystemLaunched(E_GAME_SYSTEM_TYPE.ORBITAL_CAMERA_CONTROLLER_SPAWNER) && OrbitalCameraSpawnerSystem.OrbitalCameraController != null) { //Execute only if the OrbitalCameraSpawnerSystem is launched
                OrbitalCameraSpawnerSystem.OrbitalCameraController.UseInputs = false; //Disable the inputs of the controller
            }
            
            if (GameSystemModule.IsSystemLaunched(E_GAME_SYSTEM_TYPE.TRASH_OBJECT_SPAWNER) && TrashObjectManagerSystem.LastTrashObjectHit != null) { //Execute only if the TrashObjectManagerSystem is launched
                pickupLineText.text = TrashObjectManagerSystem.LastTrashObjectHit.PickupLineLocalizedString.Trim().UppercaseFirstLetter(); //Set the localized string
            }
        }

        private void OnDisable() {
            if (GameSystemModule.IsSystemLaunched(E_GAME_SYSTEM_TYPE.ORBITAL_CAMERA_CONTROLLER_SPAWNER) && OrbitalCameraSpawnerSystem.OrbitalCameraController != null) { //Execute only if the OrbitalCameraSpawnerSystem is launched
                OrbitalCameraSpawnerSystem.OrbitalCameraController.UseInputs = true; //Enable the inputs of the controller
            }
        }

        private void Update() {
            //TODO: Please, change this...
            if(Input.GetKeyDown(KeyCode.Return)) UIWidgetSystem.DisableUIWidget(E_UI_WIDGET_TYPE.TRASH_OBJECT_WORLD_IMPACT_DESC_SCREEN);
        }

        #endregion

    }
    
}