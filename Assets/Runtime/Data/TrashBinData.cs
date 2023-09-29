using Oikos.Types;
using UnityEngine;
using UnityEngine.Localization;

namespace Oikos.Data {

    public class TrashBinData : ScriptableObject {

        #region Attributes

        [SerializeField, Tooltip("The internal name of this trash bin.")] private string internalBinName = null;

        [Header("Trash Bin Definition")] 
        [SerializeField, Tooltip("The identifier of this trash bin.")] private E_TRASHBIN_IDENTIFIER identifier = E_TRASHBIN_IDENTIFIER.UNKNOWN;

        [Header("Translation References")] 
        [SerializeField, Tooltip("The trash bin's name (Localized String).")] private LocalizedString trashBinNameLocalizedString = null;
        
        #endregion

        #region Properties

        /// <summary>
        /// The internal name of this Trash Bin.
        /// </summary>
        public string InternalName { get { return internalBinName; } }

        /// <summary>
        /// The trash bin's identifier.
        /// </summary>
        public E_TRASHBIN_IDENTIFIER Identifier { get { return identifier; } }

        /// <summary>
        /// The name of this trash bin (This string is already translated in the correct language).
        /// </summary>
        public string NameLocalizedString { get { return trashBinNameLocalizedString.GetLocalizedString(); } }
        
        #endregion
        
    }
    
}
