using UnityEngine;

namespace Oikos.Core.UI {

    public class RuntimeUIWidget {

        #region Properties

        /// <summary>
        /// The Prefab's instance of this UIWidget.
        /// </summary>
        public GameObject Instance { get; private set; } = null;
        /// <summary>
        /// The identifier of this UIWidget.
        /// The identifier is defined by the "E_UI_WIDGET_TYPE" Enum.
        /// </summary>
        public E_UI_WIDGET_TYPE Identifier { get; private set; } = E_UI_WIDGET_TYPE.UNKNOWN;
        /// <summary>
        /// The string identifier of this UIWidget.
        /// </summary>
        public string StringIdentifier { get; private set; } = string.Empty;
        /// <summary>
        /// Is this UIWidget enabled and visible to the main camera.
        /// </summary>
        public bool IsEnabled { get; private set; } = true;

        #endregion

        #region RuntimeUIWidget's methods

        public RuntimeUIWidget(UIWidgetDefinition _uiWidgetDefinition, bool _disableOnCreate = true) {
            //Instantiate the UIWidget's prefab
            Instance = Object.Instantiate(_uiWidgetDefinition.Prefab);
            if(_uiWidgetDefinition.KeepAlive) Object.DontDestroyOnLoad(Instance);
            
            if (_disableOnCreate) DisableUIWidget(); else EnableUIWidget();
            
            //Set the UIWidget's identifiers
            Identifier = _uiWidgetDefinition.Identifier;
            StringIdentifier = _uiWidgetDefinition.IdentifierString;
        }

        /// <summary>
        /// Enable the UIWidget.
        /// </summary>
        public void EnableUIWidget() {
            if (Instance == null) return;
            
            Instance.SetActive(true);
            IsEnabled = true;
            
            Logger.Trace($"Runtime UI Widget", $"UI Widget '{Identifier} // {StringIdentifier}' has been enabled!");
        }

        /// <summary>
        /// Disable the UIWidget.
        /// </summary>
        public void DisableUIWidget() {
            if (Instance == null) return;
            
            Instance.SetActive(false);
            IsEnabled = false;
            
            Logger.Trace($"Runtime UI Widget", $"UI Widget '{Identifier} // {StringIdentifier}' has been disabled!");
        }
        
        #endregion

    }
    
}