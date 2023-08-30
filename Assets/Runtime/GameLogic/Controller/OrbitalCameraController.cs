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
        
        [Header("Orbital Camera Controller - Settings (Will ported to a SO)")]
        [SerializeField] private float minXLookAngle = -60;
        [SerializeField] private float maxXLookAngle = 60;
        [SerializeField, Range(0, 250), Tooltip("The minimum distance the camera can get from the target transform.")] private float minZoom = 10;
        [SerializeField, Range(0, 250), Tooltip("The maximum distance the camera can get from the target transform.")] private float maxZoom = 50;
        [SerializeField, Range(0, 10), Tooltip("The smoothing applied when zooming.")] private float zoomSmoothing = 0.25f;
        [SerializeField] private float movementSmoothing = 0.015f;
        
        [Header("Orbital Camera Controller - Sensitivity (Will be ported to SysSettings)")]
        [SerializeField] private float mouseSensitivity = 1f;
        [SerializeField] private float zoomSensitivity = 1f;
        
        #endregion

        #region Runtime Values

        /// <summary>
        /// The X Rotation of the camera
        /// </summary>
        private float xRotation = 0f;
        /// <summary>
        /// The Y Rotation of the camera
        /// </summary>
        private float yRotation = 0f;
        
        /// <summary>
        /// The current rotation
        /// </summary>
        private Vector3 currentRotation = Vector3.zero;
        /// <summary>
        /// Velocity of the movement (Smoothing)
        /// </summary>
        private Vector3 smoothingMovementVelocity = Vector3.zero;
        /// <summary>
        /// Velocity of the zoom (Smoothing)
        /// </summary>
        private float smoothingZoomVelocity = 0f;

        #endregion

        #region Properties

        /// <summary>
        /// The target to rotate around
        /// </summary>
        public Vector3 Target { get; set; } = Vector3.zero;
        
        /// <summary>
        /// The zoom of the camera
        /// </summary>
        public float Zoom { get; private set; } = 0;

        #endregion

        #region ACameraEntity's MonoBehaviour's methods

        private protected override void OnAwakeEntity() {
            //Check references
            if(cameraAnchor == null) { Logger.TraceError("Orbital Camera Controller", "There's no Camera Anchor Transform assigned! This component will be destroyed to avoid useless logger's calls"); Destroy(this); }
            if(target != null) { Target = target.position; } else { Logger.Trace("Orbital Camera Controller", "There's no target to rotate around assigned! 'Vector3.zero' will be used instead."); }
            if(minZoom > maxZoom) { Logger.TraceWarning("Orbital Camera Controller", $"The min zoom value is greater than the max zoom value! (Min zoom: {minZoom} // Max zoom: {maxZoom}). This component will continue to run."); }
            
            //Setup this Camera Entity
            CameraName = "Orbital Camera Controller";
            CameraType = E_CAMERA_ENTITY_TYPE.CONTROLLER;
            
            //Set the default zoom value
            Zoom = maxZoom - minZoom;
        }
        
        private protected override void OnLateUpdateEntity() {
            ZoomBehaviour();
            
            if(Input.GetMouseButton(1)) CameraMovementBehaviour();
        }

        #endregion
        
        #region OrbitalCameraController's methods

        /// <summary>
        /// Move the camera from the inputs
        /// </summary>
        private void CameraMovementBehaviour() {
            float _mouseXInput = Input.GetAxis("Mouse X") * mouseSensitivity;
            float _mouseYInput = Input.GetAxis("Mouse Y") * mouseSensitivity;
            
            yRotation += _mouseXInput;
            xRotation -= _mouseYInput;
            
            xRotation = Mathf.Clamp(xRotation, minXLookAngle, maxXLookAngle);
            
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(xRotation, yRotation), ref smoothingMovementVelocity, movementSmoothing);
            transform.localEulerAngles = currentRotation;
        }
        
        /// <summary>
        /// Handle the Zoom behaviour
        /// </summary>
        private void ZoomBehaviour() {
            float _scrollValue = Input.GetAxis("Mouse ScrollWheel"); //Get the Scroll Wheel value
            
            Zoom -= _scrollValue * zoomSensitivity;
            Zoom = Mathf.Clamp(Zoom, minZoom, maxZoom); //Clamp the zoom value
            CameraComponent.fieldOfView = Mathf.SmoothDamp(CameraComponent.fieldOfView, Zoom, ref smoothingZoomVelocity, zoomSmoothing);
        }
        
        #endregion
        
    }
    
}