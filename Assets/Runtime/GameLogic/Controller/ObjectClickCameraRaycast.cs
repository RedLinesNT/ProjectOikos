using System;
using Oikos.Core;
using Oikos.GameLogic.Interactable;
using UnityEngine;
using UnityEngine.Events;
using Logger = Oikos.Core.Logger;

namespace Oikos.GameLogic.Controller {

    /// <summary>
    /// Allows you to click on objects (GameObjects) with the mouse cursor.
    /// </summary>
    /// <description>
    /// You can specify the layers of objects that can be clicked in the editor or directly by script.
    /// </description>
    public class ObjectClickCameraRaycast : MonoBehaviour {

        #region Attributes

        [Header("References")]
        [SerializeField, Tooltip("The Camera Controller")] private ACameraEntity cameraController = null;
        [SerializeField, Tooltip("The layers of objects that can be clicked on.")] private LayerMask maskHit = default;
        
        [Header("Events")]
        [SerializeField, Tooltip("The event is triggered when a valid object has been clicked/hit on.")] private UnityEvent<GameObject> onValidObjectHitEvent = null;
        [SerializeField, Tooltip("The event is triggered when a invalid object has been clicked/hit on.")] private UnityEvent<GameObject> onInvalidObjectHitEvent = null;
        
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
        
        /// <summary>
        /// The current APointerClickableObject hovered
        /// </summary>
        private APointerClickableObject currentPointerClickableObject = null;
        
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
            ShootRaycast();
            if(Input.GetMouseButtonDown(0)) currentPointerClickableObject?.OnPointerClickInternal();
        }

        #endregion

        #region ObjectClickCameraRaycast's methods

        /// <summary>
        /// Shoot a raycast from the Camera to the Mouse position in world Coordinates
        /// </summary>
        private void ShootRaycast() {
            ray = cameraController.CameraComponent.ScreenPointToRay(Input.mousePosition); //Create the ray
            
            if(Physics.Raycast(ray, out rayHit, 500f)) { //If the Raycast hit something
                if(((1<<rayHit.transform.gameObject.layer) & maskHit) != 0) { //The object hit has the correct layer
                    if (rayHit.transform.gameObject == null) return;
                    
                    onValidObjectHit?.Invoke(rayHit.transform.gameObject); //Trigger the Action event
                    onValidObjectHitEvent?.Invoke(rayHit.transform.gameObject); //Trigger the Unity Event
                    
                    //TODO: Change this logic stuff.
                    //Peak dog-shit code (YandereDev-tier...)
                    if(currentPointerClickableObject != rayHit.transform.GetComponent<APointerClickableObject>()) {
                        currentPointerClickableObject?.OnPointerExitInternal(); //Trigger the exit event
                        
                        currentPointerClickableObject = rayHit.transform.GetComponent<APointerClickableObject>(); //Set the new value
                        
                        currentPointerClickableObject?.OnPointerEnterInternal(); //Trigger the enter event
                    }
                } else { //The object hit has the wrong layer 
                    onInvalidObjectHit?.Invoke(rayHit.transform.gameObject); //Trigger the Action event
                    onInvalidObjectHitEvent?.Invoke(rayHit.transform.gameObject); //Trigger the Unity Event
                    
                    currentPointerClickableObject?.OnPointerExitInternal(); //Trigger the exit event
                    currentPointerClickableObject = null;
                }
            }
        }
        
        private void EventReceiver(GameObject _eventParam) {
            
        }

        #endregion
        
    }
    
}