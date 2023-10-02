using Oikos.Data;
using Oikos.GameLogic.Interactable;
using Oikos.Types;
using UnityEngine;
using Logger = Oikos.Core.Logger;

namespace Oikos.GameLogic.Props.Spawners {
    
    public class TrashObjectSpawnerPoint : MonoBehaviour {

        #region Attributes

        //[Header("Specific spawner settings")]
        //[SerializeField, Tooltip("If set to true, only the wanted object property will allowed to spawn on this point .")] private bool useWantedObject = false;
        //[SerializeField, Tooltip("If the property 'UseWantedObject' is true, this point will only be able to instantiate the type specified here.")] private E_TRASH_OBJECT_TYPE wantedObject = E_TRASH_OBJECT_TYPE.UNKNOWN;
        
        #endregion

        #region Runtime values

        /// <summary>
        /// The constraints of the TrashObject's rigidbody.
        /// </summary>
        private TrashObjectSpawnerRigidbodyConstraintsOverride rigidbodyConstraintsOverride = null;

        #endregion

        #region Properties

        /// <summary>
        /// The instantiated trash object prefab.
        /// </summary>
        public InteractableTrashobject TrashObjectInstance { get; private set; }
        
        /// <summary>
        /// If set to true, only the wanted object property will allowed to spawn on this point.
        /// </summary>
        public bool UseWantedObject { get { return false; } }
        
        /// <summary>
        /// If the property "UseWantedObject" is true, this point will only be able to instantiate the type specified here.
        /// </summary>
        public E_TRASH_OBJECT_TYPE WantedObject { get { return E_TRASH_OBJECT_TYPE.UNKNOWN; } }

        #endregion
        
        #region TrashObjectSpawnerPoint's methods

        /// <summary>
        /// Instantiate a TrashObject on this spawn point.
        /// </summary>
        /// <param name="_trashObject">The TrashObject's data file</param>
        /// <param name="_overrideTrashobject">If set to true, and if there's a TrashObject already spawned, the current TrashObject will be overwritten by the new one.</param>
        /// <returns>The spawned TrashObject</returns>
        public InteractableTrashobject InstantiateTrashObject(TrashObjectData _trashObject, bool _overrideTrashobject = false) {
            if(TrashObjectInstance != null && !_overrideTrashobject) {
                Logger.TraceWarning("Trash Object Spawner", $"Unable to spawn the trash object '{_trashObject.InternalName}', there's already a trash object spawner here!");
                return null;
            }
            
            /*if(useWantedObject) { //If the object given here is not the one wanted
                if(_trashObject.Identifier != wantedObject) {
                    return null;
                }
            }*/
            
            //Spawn the trash object
            TrashObjectInstance = Instantiate(_trashObject.PickRandomPrefab(), transform.position, transform.rotation); //Instantiate the prefab
            TrashObjectInstance.TrashObjectData = _trashObject;
            //Apply the Rigidbody constraints, if there's a Rigidbody component
            if(TrashObjectInstance.RigidbodyComponent == null) return TrashObjectInstance;
            
            if(rigidbodyConstraintsOverride != null) {
                TrashObjectInstance.RigidbodyComponent.isKinematic = !rigidbodyConstraintsOverride.EnableRigidbody;
                TrashObjectInstance.RigidbodyComponent.constraints = rigidbodyConstraintsOverride.RigidbodyConstraints;
            } else {
                TrashObjectInstance.RigidbodyComponent.isKinematic = !_trashObject.EnableRigidbody;
                TrashObjectInstance.RigidbodyComponent.constraints = _trashObject.RigidbodyConstraints;
            }
            
            return TrashObjectInstance;
        }

        #endregion
        
    }
    
}