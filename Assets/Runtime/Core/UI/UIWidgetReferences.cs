using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oikos.Core.UI {

    /// <summary>
    /// This class contain the references of multiple UI Widgets.
    /// The content of this class shouldn't be modified at runtime.
    /// </summary>
    public class UIWidgetReferences : ScriptableObject {

        #region Attributes

        [SerializeField, Tooltip("The list of UI Widgets")] private UIWidgetDefinition[] references = null;

        #endregion

        #region Properties

        /// <summary>
        /// The array of every UIWidgetDefinitions references in this file.
        /// </summary>
        public UIWidgetDefinition[] References { get { return references; } }

        #endregion
        
    }

    [System.Serializable] public class UIWidgetDefinition {

        #region Attributes

        [SerializeField, Tooltip("The prefab of this UI Widget.")] private GameObject prefab = null;
        [SerializeField, Tooltip("The identifier of this UI Widget.\nThis value can be used to instantiate this UI Widget.\nWARNING: Do not use 'UNKNOWN', this type is ignored!")] private E_UI_WIDGET_TYPE identifier = E_UI_WIDGET_TYPE.UNKNOWN;
        [SerializeField, Tooltip("The string identifier of this UI Widget.\nThis value can be used to instantiate this UI Widget.")] private string identifierString = string.Empty;
        [Space(10)]
        [SerializeField, Tooltip("If set to true, this widget will be loaded at the launch of the game, but will be disabled.")] private bool preloadWidget = false;
        [SerializeField, Tooltip("If set to true, this widget will stay between scenes.")] private bool keepAlive = false;
        
        #endregion

        #region Properties

        /// <summary>
        /// The prefab of this UI Widget.
        /// </summary>
        public GameObject Prefab { get { return prefab; } }
        /// <summary>
        /// The identifier of this UI Widget.
        /// This value can be used to instantiate this UI Widget.
        /// Warning: Do not use "UNKNOWN" as a type. It'll be ignored.
        /// </summary>
        public E_UI_WIDGET_TYPE Identifier { get { return identifier; } }
        /// <summary>
        /// The string identifier of this UI Widget.
        /// This value can be used to instantiate this UI Widget.
        /// </summary>
        public string IdentifierString { get { return identifierString; } }
        /// <summary>
        /// If set to true, this UI Widget will be loaded at the launch of the game.
        /// Warning: If set to true, this UI Widget will still need to be enabled.
        /// </summary>
        public bool PreloadWidget { get { return preloadWidget; } }
        /// <summary>
        /// If set to true, the UI Widget will not be destroyed between scenes.
        /// </summary>
        public bool KeepAlive { get { return keepAlive; } }
        
        #endregion

    }

}