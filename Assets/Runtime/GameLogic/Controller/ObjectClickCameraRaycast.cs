using System;
using Oikos.Core;
using UnityEngine;
using UnityEngine.Events;
using Logger = Oikos.Core.Logger;

namespace Oikos.GameLogic.Controller {

    public class ObjectClickCameraRaycast : MonoBehaviour {

        #region Attributes

        [Header("References")]
        [SerializeField] private ACameraEntity cameraController = null;
        [SerializeField] private LayerMask maskHit = default;
        
        [Header("Events")]
        [SerializeField] private UnityEvent<GameObject> onValidObjectHitEvent = null;
        [SerializeField] private UnityEvent<GameObject> onInvalidObjectHitEvent = null;
        
        #endregion

        #region Runtime Values

        /// <summary>
        /// The ray used to click on objects
        /// </summary>
        private Ray ray = default;

        /// <summary>
        /// The RaycastHit to click on objects
        /// </summary>
        private RaycastHit rayHit = default;
        
        #endregion

        #region Events

        /// <summary>
        /// Triggered when a valid object has been hit (Object's layer are in the ones specified)
        /// </summary>
        private Action<GameObject> onValidObjectHit;
        
        /// <summary>
        /// Triggered when a invalid object has been hit (Object's layer are not in the ones specified)
        /// </summary>
        private Action<GameObject> onInvalidObjectHit;

        #endregion
        
        #region Properties

        /// <summary>
        /// The object's mask able to be clicked on.
        /// </summary>
        public LayerMask HitMasks { get { return maskHit; } set { maskHit = value; } }

        /// <summary>
        /// Triggered when a valid object has been hit (Object's layer are in the ones specified)
        /// </summary>
        public event Action<GameObject> OnValidObjectHit { add { onValidObjectHit += value; } remove { onValidObjectHit -= value; } }
        
        /// <summary>
        /// Triggered when a invalid object has been hit (Object's layer are not in the ones specified)
        /// </summary>
        public event Action<GameObject> OnInvalidObjectHit { add { onInvalidObjectHit += value; } remove { onInvalidObjectHit -= value; } }
        
        #endregion
      
        #region MonoBehaviour's methods

        private void Awake() {
            if(cameraController == null) { Logger.TraceError("Object Camera Clicker", "There's no Camera Entity assigned on this component! This component will be destroyed."); Destroy(this); }
        }

        private void Update() {
            if(Input.GetMouseButtonDown(0)) ShootRaycast();
        }

        #endregion

        #region ObjectClickCameraRaycast's methods

        /// <summary>
        /// Shoot a raycast from the Camera to the Mouse position in world Coordinates
        /// </summary>
        private void ShootRaycast() {
            ray = new Ray(cameraController.CameraComponent.ScreenToWorldPoint(Input.mousePosition), cameraController.CameraComponent.gameObject.transform.forward); //Create the ray
            
            if(Physics.Raycast(ray, out rayHit, 1000f)) { //If the Raycast hit something
                if(((1<<rayHit.transform.gameObject.layer) & maskHit) != 0) { //The object hit has the correct layer
                    onValidObjectHit?.Invoke(rayHit.transform.gameObject); //Trigger the Action event
                    onValidObjectHitEvent?.Invoke(rayHit.transform.gameObject); //Trigger the Unity Event
                } else { //The object hit has the wrong layer
                    onInvalidObjectHit?.Invoke(rayHit.transform.gameObject); //Trigger the Action event
                    onInvalidObjectHitEvent?.Invoke(rayHit.transform.gameObject); //Trigger the Unity Event
                }
            }
        }

        #endregion
        
    }
    
}