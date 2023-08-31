using Oikos.Types;
using UnityEngine;

namespace Oikos.Data {

    public class TrashObjectDataContent : ScriptableObject {

        #region Attributes

        [SerializeField] private E_TRASH_OBJECT_TYPE identifier;
        [SerializeField] private string nameTranslationIdentifierString;
        [SerializeField] private string descriptionTranslationIdentifierString;
        [SerializeField] private string worldDamageDescriptionTranslationIdentifierString;
        [SerializeField] private E_TRASH_OBJECT_TYPE trashBinIdentifier;
        [SerializeField] private Sprite objectIconSprite;
        [SerializeField] private GameObject objectPrefab;

        #endregion
        
    }
    
}