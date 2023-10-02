using System;
using System.Collections;
using System.Collections.Generic;
using Oikos.Core.UI;
using Oikos.GameLogic.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Oikos.GameLogic.UI {
    
    public class TrashCenterCorrectThrowRuntimeLine : MonoBehaviour {

        public TextMeshProUGUI text = null;
        
        // Start is called before the first frame update
        private void OnEnable() {
            if (TrashCenterSystem.CurrentTrashObjectShowed == null) return;
            
            text.text = TrashCenterSystem.CurrentTrashObjectShowed.ThrowAwayLocalizedString;
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.Return)) UIWidgetSystem.DisableUIWidget(E_UI_WIDGET_TYPE.TRASH_CENTER_CORRECT_THROW);
        }
    }

    
}