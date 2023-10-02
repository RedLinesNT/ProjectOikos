using System;
using System.Collections;
using System.Collections.Generic;
using Oikos.Core.UI;
using Oikos.GameLogic.Interactable;
using Oikos.GameLogic.Props.Spawners;
using Oikos.GameLogic.Systems;
using UnityEngine;
using Logger = Oikos.Core.Logger;

namespace Oikos.GameLogic.Controller {

    public class TrashCenterCameraController : MonoBehaviour {
        
        [SerializeField, Tooltip("The point to view the Trash Object on the scene more closely during runtime.")] private Transform trashObjectViewPoint = null;
        [SerializeField] private TrashCenterSystem trashCenterMachin = null;
        [SerializeField] private ObjectClickCameraRaycast leTrucPourRaycastLesPoubelles = null;

        private InteractableTrashobject leTrucActuellementSpawned = null;
        public TrashObjectSpawnerPoint lePointDeSpawnEnDehorsDeLaMap = null;

        private bool estceQueJattendsQuiUISoitOff = false;

        private void Awake() {
            trashCenterMachin.OnShouldShowTrashObject += SpawnTrashObjectSomewhere;
            UIWidgetSystem.OnWidgetDisabled += (_widgetType) => {
                if (_widgetType == E_UI_WIDGET_TYPE.TRASH_CENTER_CORRECT_THROW && estceQueJattendsQuiUISoitOff &&
                    !trashCenterMachin.EveryTrashObjectsHasBeenShowed()) {
                    trashCenterMachin.FindNextItemToShow();
                    estceQueJattendsQuiUISoitOff = false;
                }

                if (_widgetType == E_UI_WIDGET_TYPE.TRASH_CENTER_CORRECT_THROW) leTrucPourRaycastLesPoubelles.enabled = true;
            };

            UIWidgetSystem.OnWidgetEnabled += (_widgetType) => {
                if (_widgetType == E_UI_WIDGET_TYPE.TRASH_CENTER_CORRECT_THROW) leTrucPourRaycastLesPoubelles.enabled = false;
            };
        }

        public void SpawnTrashObjectSomewhere() {
            leTrucActuellementSpawned = lePointDeSpawnEnDehorsDeLaMap.InstantiateTrashObject(TrashCenterSystem.CurrentTrashObjectShowed, true);
            
            
            //UIWidgetSystem.DisableUIWidget(E_UI_WIDGET_TYPE.TRASH_CENTER_CORRECT_THROW);
            MoveTrashObjectToViewPoint(leTrucActuellementSpawned, 1f, () => {
                Logger.Trace("Montre l'UI stp ???");
                UIWidgetSystem.EnableUIWidget(E_UI_WIDGET_TYPE.TRASH_CENTER_TRASH_INFO);
            });
        }

        public void QuandJaiHitUnTrucValid(GameObject _laPoubelle) {
            InteractableTrashBin _leComponentDeLaPoubelle = _laPoubelle.GetComponent<InteractableTrashBin>();

            if (_leComponentDeLaPoubelle == null) { //Si y a rien, j'ai probablement merdé
                Logger.Trace("La poubelle a pas le bon component?????");
                return; //Fuck it
            }

            UIWidgetSystem.DisableUIWidget(E_UI_WIDGET_TYPE.TRASH_CENTER_TRASH_INFO);
            
            bool _laBonnePoubelleQuestionMark = TrashCenterSystem.IsTrashBinValidForObject(leTrucActuellementSpawned.TrashObjectData, _leComponentDeLaPoubelle.DataFile.Identifier);

            if (!_laBonnePoubelleQuestionMark) {
                Logger.Trace("C'est pas la bonne poubelle, fais peter l'UI");
                UIWidgetSystem.EnableUIWidget(E_UI_WIDGET_TYPE.TRASH_CENTER_WRONG_THROW);
            }else {
                UIWidgetSystem.DisableUIWidget(E_UI_WIDGET_TYPE.TRASH_CENTER_WRONG_THROW);
                
                Logger.Trace("C'est la bonne poubelle askip, j'fais peter l'UI et la merde qui va avec");
                MoveTrashObjectOutOfCameraView(leTrucActuellementSpawned, 1f, () => {
                    UIWidgetSystem.EnableUIWidget(E_UI_WIDGET_TYPE.TRASH_CENTER_CORRECT_THROW);
                    estceQueJattendsQuiUISoitOff = true;
                    
                    DestroyImmediate(leTrucActuellementSpawned.gameObject); //Degage-moi ça
                    if (trashCenterMachin.EveryTrashObjectsHasBeenShowed()) {
                        Logger.Trace("Y a plus rien à montrer pélo");
                    }
                });
            }
        }
        
        #region Litteral Dogshit

        /// <summary>
        /// Move the a trash object from the scene into the TrashObjectViewPoint (Smooth).
        /// </summary>
        /// <param name="_trashObject">The TrashObject on the scene to move.</param>
        /// <param name="_time">The time to move the object.</param>
        /// <param name="_onMovementFinished">The instructions to do when the movement operation is over.</param>
        private IEnumerator MoveTrashObjectToViewPointInternal(InteractableTrashobject _trashObject, float _time, Action _onMovementFinished = null) {
            _trashObject.RigidbodyComponent.isKinematic = true; //Disable the Rigidbody
            //_trashObject.transform.parent = trashObjectViewPoint; //Set the new parent
            
            float _elapsedTime = 0f;
            Vector3 _startPos = _trashObject.gameObject.transform.position;
            Vector3 _endPos = trashObjectViewPoint.position;
            Quaternion _startRot = _trashObject.gameObject.transform.rotation;
            Quaternion _endRot = trashObjectViewPoint.rotation;
            
            while (_elapsedTime < _time) {
                _trashObject.gameObject.transform.position = Vector3.Lerp(_startPos, _endPos, (_elapsedTime / _time));
                _trashObject.gameObject.transform.rotation = Quaternion.Lerp(_startRot, _endRot, (_elapsedTime / _time));
 
                _elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            //Set the end values
            _trashObject.gameObject.transform.position = _endPos;
            _trashObject.transform.rotation = _endRot;
            
            
            _onMovementFinished?.Invoke(); //Trigger the event when finished

            yield return null;
        }
        
        /// <summary>
        /// Move the a trash object from the scene to a point not visible by the camera (Above) (Smooth).
        /// </summary>
        /// <param name="_trashObject">The TrashObject on the scene to move.</param>
        /// <param name="_time">The time to move the object.</param>
        /// <param name="_onMovementFinished">The instructions to do when the movement operation is over.</param>
        private IEnumerator MoveTrashObjectOutOfCameraViewInternal(InteractableTrashobject _trashObject, float _time, Action _onMovementFinished = null) {
            _trashObject.RigidbodyComponent.isKinematic = true; //Disable the Rigidbody
            
            float _elapsedTime = 0f;
            Vector3 _startPos = _trashObject.gameObject.transform.position;
            Vector3 _endPos = new Vector3(trashObjectViewPoint.position.x, trashObjectViewPoint.position.y + 10, trashObjectViewPoint.position.z);
            
            while (_elapsedTime < _time) {
                _trashObject.gameObject.transform.position = Vector3.Lerp(_startPos, _endPos, (_elapsedTime / _time));
 
                _elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            //Set the end values
            _trashObject.gameObject.transform.position = _endPos;
            
            _onMovementFinished?.Invoke(); //Trigger the event when finished

            yield return null;
        }

        /// <summary>
        /// <inheritdoc cref="MoveTrashObjectToViewPointInternal"/>
        /// </summary>
        /// <param name="_trashObject"><inheritdoc cref="MoveTrashObjectToViewPointInternal"/></param>
        /// <param name="_time"><inheritdoc cref="MoveTrashObjectToViewPointInternal"/></param>
        /// <param name="_onMovementFinished"><inheritdoc cref="MoveTrashObjectToViewPointInternal"/></param>
        public void MoveTrashObjectToViewPoint(InteractableTrashobject _trashObject, float _time, Action _onMovementFinished = null) {
            StartCoroutine(MoveTrashObjectToViewPointInternal(_trashObject, _time, _onMovementFinished));
        }
        
        /// <summary>
        /// <inheritdoc cref="MoveTrashObjectOutOfCameraViewInternal"/>
        /// </summary>
        /// <param name="_trashObject"><inheritdoc cref="MoveTrashObjectOutOfCameraViewInternal"/></param>
        /// <param name="_time"><inheritdoc cref="MoveTrashObjectOutOfCameraViewInternal"/></param>
        /// <param name="_onMovementFinished"><inheritdoc cref="MoveTrashObjectOutOfCameraViewInternal"/></param>
        public void MoveTrashObjectOutOfCameraView(InteractableTrashobject _trashObject, float _time, Action _onMovementFinished = null) {
            StartCoroutine(MoveTrashObjectOutOfCameraViewInternal(_trashObject, _time, _onMovementFinished));
        }
        

        #endregion
        
    }
    
}