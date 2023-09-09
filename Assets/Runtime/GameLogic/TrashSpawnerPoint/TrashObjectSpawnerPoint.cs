using Oikos.Data;
using Oikos.GameLogic.Interactable;
using Oikos.Types;
using UnityEngine;
using Logger = Oikos.Core.Logger;

namespace Oikos.GameLogic.Props.Spawners {
    
    public class TrashObjectSpawnerPoint : MonoBehaviour {

        #region Attributes

        [Header("Specific spawner settings")]
        [SerializeField, Tooltip("If set to true, the wanted object attribute will be used.")] private bool useWantedObject = false;
        [SerializeField, Tooltip("If the attribute 'UseWantedObject' is true, this spawn point will only have this specified item type.")] private E_TRASH_OBJECT_TYPE wantedObject = E_TRASH_OBJECT_TYPE.UNKNOWN;
        
        #endregion

        #region Runtime values

        /// <summary>
        /// The constraints of the TrashObject's rigidbody
        /// </summary>
        private TrashObjectSpawnerRigidbodyConstraintsOverride rigidbodyConstraintsOverride = null;

        /// <summary>
        /// The instantiated trash object prefab
        /// </summary>
        private InteractableTrashobject trashobjectInstance = null;
        
        #endregion

        #region TrashObjectSpawnerPoint's methods

        public bool InstantiateTrashobject(TrashObjectData _trashObject, bool _overrideTrashobject = false) {
            if(trashobjectInstance != null && !_overrideTrashobject) {
                Logger.TraceWarning("Trash Object Spawner", $"Unable to spawn the trash object '{_trashObject.InternalName}', there's already a trash object spawner here!");
                return false;
            }
            
            if(useWantedObject) { //If the object given here is not the one wanted
                if(_trashObject.Identifier != wantedObject) {
                    return false;
                }
            }
            
            //Spawn the trash object
            trashobjectInstance = Instantiate(_trashObject.PickRandomPrefab(), transform.position, transform.rotation); //Instantiate the prefab
            
            //Apply the Rigidbody constraints, if there's a Rigidbody component
            if(trashobjectInstance.RigidbodyComponent == null) return true;
            
            if(rigidbodyConstraintsOverride != null) {
                trashobjectInstance.RigidbodyComponent.isKinematic = !rigidbodyConstraintsOverride.EnableRigidbody;
                trashobjectInstance.RigidbodyComponent.constraints = rigidbodyConstraintsOverride.RigidbodyConstraints;
            } else {
                trashobjectInstance.RigidbodyComponent.isKinematic = !_trashObject.EnableRigidbody;
                trashobjectInstance.RigidbodyComponent.constraints = _trashObject.RigidbodyConstraints;
            }
            
            return true;
        }

        #endregion
        
    }
    
}