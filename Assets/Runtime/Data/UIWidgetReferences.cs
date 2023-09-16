using System;
using System.Collections;
using System.Collections.Generic;
using Oikos.Types;
using UnityEngine;

namespace Oikos.Data {
    
    /// <summary>
    /// This class contain the UI Widget References.
    /// The content of this class shouldn't be modified at runtime.
    /// </summary>
    public class UIWidgetReference : ScriptableObject {

        #region Attributes

        [Header("UI Widget References")]
        [SerializeField, Tooltip("The list of UI Widgets")] private UIWidget[] references = null;
        
        #endregion

        #region Properties

        /// <summary>
        /// Array of every UI Widgets referenced in this ScriptableObject file.
        /// </summary>
        public UIWidget[] References { get { return references; } }

        #endregion

        #region UIWidget class

        [Serializable] public class UIWidget {

            #region Attributes

            [SerializeField, Tooltip("The prefab of this UI Widget")] private GameObject widgetPrefab = null;
            [SerializeField, Tooltip("The UI Widget's string identifier.\nCan be used to instantiate this widget at runtime.")] private string stringIdentifier = String.Empty;
            [SerializeField, Tooltip("The UI Widget's identifier\nCan be used to instantiate this widget at runtime.")] private E_UI_WIDGET_TYPE identifier = E_UI_WIDGET_TYPE.UNKNOWN;
            
            [Header("UI Widget settings")]
            [SerializeField, Tooltip("If this value is true, this UI Widget will be loaded at the game's launch, but disabled.")] private bool preloadWidget = false;
            
            #endregion

            #region Properties

            /// <summary>
            /// The prefab of this UI Widget.
            /// </summary>
            public GameObject WidgetPrefab { get { return widgetPrefab; } }
            /// <summary>
            /// The UI Widget's string identifier.
            /// This identifier can be used to instantiate this widget at runtime.
            /// </summary>
            public string StringIdentifier { get { return stringIdentifier; } }
            /// <summary>
            /// The UI Widget's identifier. Defined by the "E_UI_WIDGET_TYPE" Enum.
            /// The identifier can be used to instantiate this widget at runtime.
            /// </summary>
            public E_UI_WIDGET_TYPE Identifier { get { return identifier; } }
            
            /// <summary>
            /// If set to true,
            /// this widget will be instantiated at the game's launch, but will be disabled.
            /// </summary>
            public bool PreloadWidget { get { return preloadWidget; } }
            
            #endregion
            
        }

        #endregion
        
    }
    
}