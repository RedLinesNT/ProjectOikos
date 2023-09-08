using System;
using UnityEngine;

namespace Oikos.Core {

    /// <summary>
    /// ACameraEntity allows a Camera to be registered within the system managing it.
    /// </summary>
    /// <description>
    /// There are different methods for classes inheriting from this one, which want to use MonoBehaviour methods such as "Start", "Update", ... 
    /// See the virtual methods of this class for more details.
    /// </description>
    /// <inEditor>
    /// You can setup the references of this instance via the Engine's editor GUI
    /// </inEditor>
    public abstract class ACameraEntity : MonoBehaviour {

        #region Attributes

        [Header("Camera Entity References")]
        [SerializeField, InspectorName("Camera Component"), Tooltip("The Camera Component to use.")] private Camera cameraOverrideField = null;
        
        #endregion
        
        #region Properties

        /// <summary>
        /// The name of this Camera Entity
        /// </summary>
        public string CameraName { get; private protected set; } = "No name - Camera Entity";
        /// <summary>
        /// The unique identifier of this Camera Entity
        /// </summary>
        public string CameraIdentifier { get; private protected set; } = Utils.GenerateUUID();
        /// <summary>
        /// The UnityEngine.Camera component of this Camera Entity
        /// </summary>
        public Camera CameraComponent { get; private protected set; } = null;
        /// <summary>
        /// The type of this Camera Entity
        /// </summary>
        public E_CAMERA_ENTITY_TYPE CameraType { get; private protected set; } = E_CAMERA_ENTITY_TYPE.UNSPECIFIED;
        
        #endregion

        #region MonoBehaviour's methods

        private void Awake() {
            CameraComponent = GetComponent<Camera>(); //Try to get the Camera component
            
            OnAwakeEntity(); //Call this method to every children
        }

        private void Start() {
            //Check if we should use the Camera Component attribute override value or the one set by script
            if(cameraOverrideField == null) {
                CameraComponent = GetComponent<Camera>(); //Try to get the Camera component
            } else {
                CameraComponent = cameraOverrideField; //Set the Override Camera
            }
            
            CameraEntitySystem.RegisterCameraEntity(this); //Register this Camera Entity
            
            OnStartEntity(); //Call this method on every children
        }

        private void OnEnable() => OnEnableEntity();

        private void OnDisable() => OnDisableEntity();

        private void OnDestroy() {
            CameraEntitySystem.UnregisterCameraEntity(this); //Unregister this Camera Entity
            
            OnDestroyEntity(); //Call this method to every children
        }
        
        private void Update() => OnUpdateEntity();
        
        private void FixedUpdate() => OnFixedUpdateEntity();

        private void LateUpdate() => OnLateUpdateEntity();

        #endregion

        #region ACameraEntity's virtual methods

        /// <summary>
        /// OnAwakeEntity is called when an enabled script instance is being loaded.
        /// </summary>
        private protected virtual void OnAwakeEntity(){}
        /// <summary>
        /// OnStartEntity is called on the frame when a script is enabled just before any of the Update methods are called for the first frame.
        /// </summary>
        private protected virtual void OnStartEntity(){}
        /// <summary>
        /// OnEnableEntity is called when the object becomes enabled and active.
        /// </summary>
        private protected virtual void OnEnableEntity(){}
        /// <summary>
        /// OnDisableEntity is called when the behaviour becomes disabled.
        /// </summary>
        private protected virtual void OnDisableEntity(){}
        /// <summary>
        /// Destroying the attached Behaviour will result in the game or Scene receiving OnDestroy.
        /// </summary>
        private protected virtual void OnDestroyEntity() {}
        /// <summary>
        /// OnUpdateEntity is called every frames.
        /// </summary>
        private protected virtual void OnUpdateEntity(){}
        /// <summary>
        /// Frame-rate independent MonoBehaviour.FixedUpdate message for physics calculations.
        /// </summary>
        private protected virtual void OnFixedUpdateEntity(){}
        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// </summary>
        private protected virtual void OnLateUpdateEntity(){}

        #endregion

        #region ACameraEntity's methods

        /// <summary>
        /// Reset the position/rotation of this Camera Entity (0,0,0)
        /// </summary>
        public void ResetTransform() {
            ResetPosition();
            ResetRotation();
        }

        /// <summary>
        /// Reset the position of this CameraEntity (0,0,0)
        /// </summary>
        public void ResetPosition() {
            transform.position = Vector3.zero;
        }
        
        /// <summary>
        /// Reset the rotation of this CameraEntity (0,0,0,0)
        /// </summary>
        public void ResetRotation() {
            transform.rotation = Quaternion.identity;
        }
        
        #endregion
        
    }
    
}