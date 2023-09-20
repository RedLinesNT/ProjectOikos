using System;
using Oikos.Core.Systems;
using Oikos.Core.UI;
using Oikos.GameLogic.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Oikos.GameLogic.UI {

    public class TrashObjectScreenInfo : MonoBehaviour {

        #region Attributes

        [Header("UI References")]
        [SerializeField, Tooltip("The TextMeshPro text component used to display the world impact of a Trash Object.")] private TextMeshProUGUI worldImpactDesc = null;
        [SerializeField, Tooltip("The UI Sprite used to show the Trash Object's icon.")] private Image trashIcon = null;

        #endregion

        #region MonoBehaviour's methods

        private void OnEnable() {
            //Don't execute anything if the TrashObjectManagerSystem isn't launched
            if (!GameSystemModule.IsSystemLaunched(E_GAME_SYSTEM_TYPE.TRASH_OBJECT_SPAWNER) || TrashObjectManagerSystem.LastTrashObjectHit == null) return;

            worldImpactDesc.text = TrashObjectManagerSystem.LastTrashObjectHit.WorldImpactDescriptionLocalizedString; //Set the localized string
            trashIcon.sprite = TrashObjectManagerSystem.LastTrashObjectHit.TrashIcon; //Set the TrashObject's sprite icon
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.Escape)) UIWidgetSystem.DisableUIWidget(E_UI_WIDGET_TYPE.TRASH_OBJECT_WORLD_IMPACT_DESC_SCREEN);
        }

        #endregion

    }
    
}