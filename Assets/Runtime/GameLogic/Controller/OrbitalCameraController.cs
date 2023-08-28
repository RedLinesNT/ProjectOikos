using System.Collections;
using System.Collections.Generic;
using Oikos.Core;
using UnityEngine;
using Logger = Oikos.Core.Logger;

namespace Oikos.GameLogic.Controller {

    public class OrbitalCameraController : ACameraEntity {

        #region Attributes

        [Header("Orbital Camera Controller - References")]
        [SerializeField, InspectorName("Target to rotate around"), Tooltip("The Transform to rotate around.\nIf not specified, Vector3.Zero will be taken")] private Transform target = null;
        [SerializeField, InspectorName("Camera's anchor/parent"), Tooltip("The Transform of the anchor/parent of the Camera")] private Transform cameraAnchor = null;
        
        #endregion

        #region Runtime Values

        /// <summary>
        /// The previous rotation of the Camera Controller (this)
        /// </summary>
        private Vector3 previousPosition = Vector3.zero;

        #endregion

        #region Properties

        /// <summary>
        /// The target to rotate around
        /// </summary>
        public Vector3 Target { get; set; } = Vector3.zero;

        #endregion

        #region ACameraEntity's MonoBehaviour's methods

        private protected override void OnAwakeEntity() {
            //Check references
            if(cameraAnchor == null) { Logger.TraceError("Orbital Camera Controller", "There's no Camera Anchor Transform assigned! This component will be destroyed to avoid useless logger's calls"); Destroy(this); }
            if(target != null) { Target = target.position; } else { Logger.Trace("Orbital Camera Controller", "There's no target to rotate around assigned! 'Vector3.zero' will be used instead."); }
            
            //Setup this Camera Entity
            CameraName = "Orbital Camera Controller";
            CameraType = E_CAMERA_ENTITY_TYPE.CONTROLLER;
        }

        private protected override void OnLateUpdateEntity() {
           if(Input.GetMouseButtonDown(0)) BeginCameraMovement();
           if(Input.GetMouseButton(0)) CameraMovementBehaviour();
        }

        #endregion
        
        #region OrbitalCameraController's methods

        /// <summary>
        /// Start the camera movement
        /// </summary>
        private void BeginCameraMovement() {
            previousPosition = CameraComponent.ScreenToViewportPoint(Input.mousePosition); //Get the position of the mouse in coords
        }
        
        /// <summary>
        /// Move the camera from the inputs
        /// </summary>
        private void CameraMovementBehaviour() {
            Vector3 _direction = previousPosition - CameraComponent.ScreenToViewportPoint(Input.mousePosition);
            
            cameraAnchor.position = Target; //Set the correct original position
            
            //Rotate based on the mouse inputs
            cameraAnchor.Rotate(new Vector3(1, 0, 0), _direction.y * 180);
            cameraAnchor.Rotate(new Vector3(0, 1, 0), -_direction.x * 180, Space.World);
            cameraAnchor.Translate(new Vector3(0, 0, -30));
            
            previousPosition = CameraComponent.ScreenToViewportPoint(Input.mousePosition); //Get the position of the mouse in coords
        }

        #endregion
        
    }
    
}