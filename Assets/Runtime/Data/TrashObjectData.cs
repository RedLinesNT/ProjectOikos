using Oikos.Types;
using UnityEngine;

namespace Oikos.Data {

    public class TrashObjectData : ScriptableObject {

        #region Attributes

        [Header("Trash Item Definition")]
        [SerializeField] private E_TRASH_OBJECT_TYPE identifier;
        [SerializeField] private E_TRASHBIN_IDENTIFIER trashBinIdentifier;
        [SerializeField] private Sprite objectIconSprite;
        [SerializeField] private GameObject objectPrefab;
        
        [Header("Translation references")]
        [SerializeField] private string nameTranslationIdentifierString;
        [SerializeField] private string descriptionTranslationIdentifierString;
        [SerializeField] private string worldDamageDescriptionTranslationIdentifierString;

        #endregion
        
    }
    
}