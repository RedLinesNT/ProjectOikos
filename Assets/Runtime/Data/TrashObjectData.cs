using Oikos.GameLogic.Interactable;
using Oikos.Types;
using UnityEngine;
using UnityEngine.Localization;
using Random = System.Random;

namespace Oikos.Data {

    public class TrashObjectData : ScriptableObject {

        #region Attributes

        [SerializeField, Tooltip("The internal name of this item.")] private string internalItemName = null;
        
        [Header("Trash Item Definition")]
        [SerializeField, Tooltip("The identifier of this trash object.")] private E_TRASH_OBJECT_TYPE identifier = E_TRASH_OBJECT_TYPE.UNKNOWN;
        [SerializeField, Tooltip("The identifier of the target trash bin of this object.")] private E_TRASHBIN_IDENTIFIER trashBinIdentifier = E_TRASHBIN_IDENTIFIER.UNKNOWN;
        [SerializeField, Tooltip("The icon used to show the item into the UI.")] private Sprite spriteIcon = null;
        [SerializeField, Tooltip("The Prefab of this trash object.")] private InteractableTrashobject[] trashModelPrefabs = null;
        
        [Header("Translation references")]
        [SerializeField, Tooltip("The trash's name (Localized string).")] private LocalizedString trashNameLocalizedString = null;
        [SerializeField, Tooltip("The trash's type name (Localized string).")] private LocalizedString trashTypeLocalizedString = null;
        [SerializeField, Tooltip("The trash's lifespan (Localized string).")] private LocalizedString trashLifespanLocalizedString = null;
        [SerializeField, Tooltip("The trash's world impact description (Localized string).")] private LocalizedString worldImpactDescriptionLocalizedString = null;
        [SerializeField, Tooltip("The text displayed when picking up this trash object (Localized string).")] private LocalizedString trashPickupLocalizedString = null;
        [SerializeField, Tooltip("The text displayed when throwing away this trash object in the correct trash-bin (Localized string).")] private LocalizedString trashThrowAwayLocalizedString = null;
        
        [Header("Trash Item Physics settings")]
        [SerializeField, Tooltip("Enable the item's rigidbody. This will work only if a Rigidbody Component can be found.")] private bool enableRigidbody = true;
        [SerializeField, Tooltip("Set physics constraints of this trash item's rigidbody.\nEnableRigidbody must be set to true.")] private RigidbodyConstraints rigidbodyConstraints = RigidbodyConstraints.None;
        
        #endregion

        #region Properties
        
        /// <summary>
        /// The internal name of this trash item.
        /// </summary>
        public string InternalName { get { return internalItemName; } }

        /// <summary>
        /// The identifier of this item. Defined by the "E_TRASH_OBJECT_TYPE" Enum.
        /// </summary>
        public E_TRASH_OBJECT_TYPE Identifier { get { return identifier; } }
        /// <summary>
        /// The identifier of the target trash bin of this item. Defined by the "E_TRASHBIN_IDENTIFIER" Enum.
        /// </summary>
        public E_TRASHBIN_IDENTIFIER TrashbinIdentifier { get { return trashBinIdentifier; } }
        /// <summary>
        /// The icon of this trash object.
        /// Can be used to display it into the UI for example.
        /// </summary>
        public Sprite TrashIcon { get { return spriteIcon; } }
        /// <summary>
        /// The model prefab of this trash object.
        /// </summary>
        public InteractableTrashobject[] ModelPrefabs { get { return trashModelPrefabs; } }
        
        /// <summary>
        /// The name of this trash object (This string is already translated in the correct language).
        /// </summary>
        public string NameLocalizedString { get { return trashNameLocalizedString.GetLocalizedString(); } }
        /// <summary>
        /// The name of the trash type of this trash object (This string is already translated in the correct language).
        /// </summary>
        public string TrashTypeLocalizedString { get { return trashTypeLocalizedString.GetLocalizedString(); } }
        /// <summary>
        /// The lifespan of this trash object (This string is already translated in the correct language).
        /// </summary>
        public string LifespanLocalizedString { get { return trashLifespanLocalizedString.GetLocalizedString(); } }
        /// <summary>
        /// The world impact's description of this trash object (This string is already translated in the correct language).
        /// </summary>
        public string WorldImpactDescriptionLocalizedString { get { return worldImpactDescriptionLocalizedString.GetLocalizedString(); } }
        /// <summary>
        /// The text displayed when picking up this trash object (This string is already translated in the correct language).
        /// </summary>
        public string PickupLineLocalizedString { get { return trashPickupLocalizedString.GetLocalizedString(); } }
        /// <summary>
        /// The text displayed when throwing away this trash object in the correct trash-bin (This string is already translated in the correct language).
        /// </summary>
        public string ThrowAwayLocalizedString { get { return trashThrowAwayLocalizedString.GetLocalizedString(); } }
        
        /// <summary>
        /// Enable the item's rigidbody.
        /// This will work only if a Rigidbody Component can be found.
        /// </summary>
        public bool EnableRigidbody { get { return enableRigidbody; } }
        /// <summary>
        /// The Rigidbody's physics constraints.
        /// EnableRigidbody must be set to true in order to be taken in account.
        /// </summary>
        public RigidbodyConstraints RigidbodyConstraints { get { return rigidbodyConstraints; } }
        
        #endregion

        #region TrashobjectData's methods

        /// <summary>
        /// Will pick a random object prefabs among the list of given one.
        /// </summary>
        public InteractableTrashobject PickRandomPrefab() {
            Random _random = new Random();
            return trashModelPrefabs[_random.Next(0, trashModelPrefabs.Length)];
        }

        #endregion
        
    }
    
}