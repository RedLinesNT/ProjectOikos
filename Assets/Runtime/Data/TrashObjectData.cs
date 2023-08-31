using Oikos.Types;
using UnityEngine;
using UnityEngine.Localization;

namespace Oikos.Data {

    public class TrashObjectData : ScriptableObject {

        #region Attributes

        [Header("Trash Item Definition")]
        [SerializeField] private E_TRASH_OBJECT_TYPE identifier;
        [SerializeField] private E_TRASHBIN_IDENTIFIER trashBinIdentifier;
        [SerializeField] private Sprite objectIconSprite;
        [SerializeField] private GameObject objectPrefab;
        
        [Header("Translation references")]
        [SerializeField] private LocalizedString nameTranslationIdentifierString;
        [SerializeField] private LocalizedString descriptionTranslationIdentifierString;
        [SerializeField] private LocalizedString worldDamageDescriptionTranslationIdentifierString;
        
        #endregion
        
    }
    
}