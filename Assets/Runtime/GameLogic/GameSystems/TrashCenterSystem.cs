using System;
using System.Collections.Generic;
using System.Linq;
using Oikos.Core.SceneManagement;
using Oikos.Core.Systems;
using Oikos.Data;
using Oikos.Types;
using UnityEngine;
using UnityEngine.Events;
using Logger = Oikos.Core.Logger;

namespace Oikos.GameLogic.Systems {

    public class TrashCenterSystem : MonoBehaviour {

        #region Attributes

        /// <summary>
        /// Array of the trash objects showed in the previous gameplay scene
        /// </summary>
        public List<TrashObjectData> objectsFromPreviousScene = null;

        private List<TrashObjectData> laVraiListMdrrr = null;

        #endregion
        
        #region Events

        [Header("Trash Center System - Events")]
        [SerializeField] private UnityEvent onShouldShowTrashObjectEditor = null;
        [SerializeField] private UnityEvent onFailedThrowTrashObjectEditor = null;
        [SerializeField] private UnityEvent onSuccessThrowTrashObjectEditor = null;
        
        /// <summary>
        /// This event is triggered when a Trash Object should be showed
        /// </summary>
        private Action onShouldShowTrashObject;
        
        #endregion

        public SceneGameplayData dataTruc;
        
        #region Properties

        /// <summary>
        /// The current Trash Object that should be showed.
        /// </summary>
        public static TrashObjectData CurrentTrashObjectShowed { get; private set; } = null;
        
        /// <summary>
        /// Triggered when a trash object should be showed to the player
        /// </summary>
        public event Action OnShouldShowTrashObject {
            add { onShouldShowTrashObject += value; }
            remove { onShouldShowTrashObject -= value; }
        }
        
        #endregion

        #region MonoBehaviour's methods

        private void Awake() {
            TrashObjectData[] trucs = null;
            objectsFromPreviousScene = dataTruc.TrashObjects.ToList();
            laVraiListMdrrr = new List<TrashObjectData>(objectsFromPreviousScene);

            for (int i=0; i<trucs.Length; i++) {
                Logger.Trace("Trash Center System", $"Trash Object '{trucs[i].InternalName}' ({trucs[i].Identifier}) has been found");
            }
        }

        private void Start() {
            FindNextItemToShow();
        }

        #endregion
        
        #region TrashCenterSystem's Methods

        /// <summary>
        /// Find the next trash object to show to the player
        /// </summary>
        public void FindNextItemToShow() {
            if(objectsFromPreviousScene.Count <= 0) { //There's no items left to show
                Logger.Trace("Trash Center System", "Every trash object has been showed.");
                return; 
            }
            
            
            CurrentTrashObjectShowed = laVraiListMdrrr.Find(_item => _item == objectsFromPreviousScene.First());
            onShouldShowTrashObject?.Invoke(); //Trigger the internal event
            onShouldShowTrashObjectEditor?.Invoke(); //Trigger the editor event
            
            Logger.Trace("Trash Center System", $"Showing Trash Object '{CurrentTrashObjectShowed.InternalName}' ({CurrentTrashObjectShowed.Identifier})");
            
            //Remove the element
            objectsFromPreviousScene.Remove(objectsFromPreviousScene.First());
        }

        /// <summary>
        /// Returns true if the trash object given is thrown into the correct trash bin
        /// </summary>
        public static bool IsTrashBinValidForObject(TrashObjectData _trashObjectData, E_TRASHBIN_IDENTIFIER _trashBinIdentifier) {
            return _trashObjectData.TrashbinIdentifier == _trashBinIdentifier;
        }

        /// <summary>
        /// Returns false if there's still a trash object to show to the player
        /// </summary>
        /// <returns></returns>
        public bool EveryTrashObjectsHasBeenShowed() {
            return objectsFromPreviousScene.Count <= 0;
        }
        
        #endregion
        
    }
    
}