using System;
using UnityEngine;
using UnityEngine.Events;

namespace Oikos.GameLogic.Interactable {
    
    public abstract class APointerClickableObject : MonoBehaviour {

        #region Attributes

        [Header("Pointer Clickable Object - Settings")]
        [SerializeField, Tooltip("If set to false, this object will receive interactable events.")] private bool isInteractable = true;
        
        [Header("Pointer Clickable Object - Events")]
        [SerializeField, Tooltip("This event will be triggered when the user's pointer will be on this object.")] private UnityEvent onPointerEnterEditor = null;
        [SerializeField, Tooltip("This event will be triggered when the user's pointer will leave this object.")] private UnityEvent onPointerExitEditor = null;
        [SerializeField, Tooltip("This event will be triggered when the user's pointer will click on this object.")] private UnityEvent onPointerClickEditor = null;
        
        #endregion
        
        #region Events

        /// <summary>
        /// Internal event triggered when the user's pointer is on this object.
        /// </summary>
        private Action onPointerEnter;
        
        /// <summary>
        /// Internal event triggered when the user's pointer leaved this object.
        /// </summary>
        private Action onPointerExit;
        
        /// <summary>
        /// Internal event triggered whe the user's pointer clicked on this object.
        /// </summary>
        private Action onPointerClick;

        #endregion

        #region Properties

        /// <summary>
        /// Is this object interactable
        /// </summary>
        public bool IsInteractable { get { return isInteractable; } set { isInteractable = value; if(!isInteractable) OnPointerExitInternal(); } }
        
        /// <summary>
        /// This event is triggered when the user's pointer is on this object.
        /// </summary>
        public event Action OnPointerEnterEvent { add { onPointerEnter += value; } remove { onPointerEnter -= value; } }

        /// <summary>
        /// This event is triggered when the user's pointer leave this object.
        /// </summary>
        public event Action OnPointerExitEvent { add { onPointerExit += value; } remove { onPointerExit -= value; } }
        
        /// <summary>
        /// This event is triggered when the user's pointer clicked this object.
        /// </summary>
        public event Action OnPointerClickEvent { add { onPointerClick += value; } remove { onPointerClick -= value; } }
        
        #endregion
        
        #region APointerClickableObject's virtual methods

        /// <summary>
        /// OnPointerEnter is triggered when the user's pointer is on this object.
        /// </summary>
        /// <description>
        /// This event is usually triggered by the "ObjectClickCameraRaycast" Component setup on the game's main camera.
        /// </description>
        private protected virtual void OnPointerEnter(){}

        /// <summary>
        /// OnPointerExit is triggered when the user's pointer has leaved this object.
        /// </summary>
        /// <description>
        /// This event is usually triggered by the "ObjectClickCameraRaycast" Component setup on the game's main camera.
        /// </description>
        private protected virtual void OnPointerExit(){}
        
        /// <summary>
        /// OnPointerClick is triggered when the user's pointer clicked in this object.
        /// </summary>
        /// <description>
        /// This event is usually triggered by the "ObjectClickCameraRaycast" Component setup on the game's main camera.
        /// </description>
        private protected virtual void OnPointerClick(){}
        
        #endregion

        #region APointerClickableObject's methods

        /// <summary>
        /// Trigger the "OnPointerEnter" to every children using this abstract class.
        /// </summary>
        public void OnPointerEnterInternal() {
            if(!isInteractable) return;
            
            onPointerEnterEditor?.Invoke(); //Trigger the events setup in the editor
            onPointerEnter?.Invoke(); //Trigger the internal event
            
            OnPointerEnter(); //Call this method to every children
        }
        
        /// <summary>
        /// Trigger the "OnPointerExit" to every children using this abstract class.
        /// </summary>
        public void OnPointerExitInternal() {
            if(!isInteractable) return;
            
            onPointerExitEditor?.Invoke(); //Trigger the events setup in the editor
            onPointerExit?.Invoke(); //Trigger the internal event
            
            OnPointerExit(); //Call this method to every children
        }
        
        /// <summary>
        /// Trigger the "OnPointerClick" to every children using this abstract class.
        /// </summary>
        public void OnPointerClickInternal() {
            if(!isInteractable) return;
            
            onPointerClickEditor?.Invoke(); //Trigger the events setup in the editor
            onPointerClick?.Invoke(); //Trigger the internal event
            
            OnPointerClick(); //Call this method to every children
        }

        #endregion
        
    }
    
}