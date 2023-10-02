using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEngine.EventSystems;

namespace Oikos.Core.UI {

    public static class UIWidgetSystem {

        #region Attributes

        /// <summary>
        /// List of every "UIWidgetDefinition" referenced inside "UIWidgetReferences".
        /// </summary>
        private static List<UIWidgetDefinition> widgets = new List<UIWidgetDefinition>();
        /// <summary>
        /// List of every instantiated UIWidgets
        /// </summary>
        private static List<RuntimeUIWidget> widgetsInstance = new List<RuntimeUIWidget>();

        #endregion

        #region Events

        /// <summary>
        /// Triggered when a UI Widget has been enabled
        /// </summary>
        private static Action<E_UI_WIDGET_TYPE> onWidgetEnabled;

        /// <summary>
        /// Triggered when a UI Widget has been disabled
        /// </summary>
        private static Action<E_UI_WIDGET_TYPE> onWidgetDisable;
        
        #endregion

        #region Properties

        public static event Action<E_UI_WIDGET_TYPE> OnWidgetEnabled {
            add { onWidgetEnabled += value; }
            remove { onWidgetEnabled -= value; }
        }
        
        public static event Action<E_UI_WIDGET_TYPE> OnWidgetDisabled {
            add { onWidgetDisable += value; }
            remove { onWidgetDisable -= value; }
        }

        #endregion

        #region UIWidgetSystem's internal methods

        /// <summary>
        /// Called at the launch of the game.
        /// Used to load the required content.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)] private static void Initialize() {
            LoadUIWidgetReferencesInternal();
            
            //Create a new event system
            GameObject _eventSystem = new GameObject("UI - Event System", typeof(EventSystem), typeof(StandaloneInputModule));
            Object.DontDestroyOnLoad(_eventSystem); //Uhhhh
        }

        /// <summary>
        /// Get every UIWidgetDefinition referenced and instantiate the required ones.
        /// </summary>
        private static void LoadUIWidgetReferencesInternal() {
            UIWidgetReferences[] _referencesFiles = ResourceFetcher.GetResourceFilesFromType<UIWidgetReferences>(); //Get every UIWidgetReferences files inside the game
            
            //Don't execute anything if no files was found
            if (_referencesFiles is not { Length: > 0 }) {
                Logger.TraceWarning("UI Widget System", "Couldn't find any UIWidgetReferences files in the game during the first initialization! Aborting initialization.");
                return;
            }

            int _preloadedWidgetsCount = 0; //Used to trace the number of UIWidgets preloaded

            for (int i=0; i<_referencesFiles.Length; i++) { //Loop into every UIWidgetReferences files found
                for (int y=0; y<_referencesFiles[i].References.Length; y++) { //Loop into every UIWidgetReferences inside the current file
                    if (_referencesFiles[i].References[y].Prefab == null) return; //Check if the prefab is null
                    
                    widgets.Add(_referencesFiles[i].References[y]); //Add the UIWidgetDefinition to the internal list
                }
            }
            
            //Preload required widgets
            for (int i=0; i<widgets.Count; i++) {
                if (widgets[i].PreloadWidget) { //If this UIWidgetDefinition should be preloaded...
                    widgetsInstance.Add(new RuntimeUIWidget(widgets[i])); //Create a new RuntimeUIWidget instance, and register it

                    _preloadedWidgetsCount++;
                }
            }
            
            Logger.Trace($"UI Widget System", $"Found '{widgets.Count}' UIWidgetDefinition and preloaded '{_preloadedWidgetsCount}' among them.");
        }
        
        #endregion

        #region UIWidgetSystem's external methods

        /// <summary>
        /// Disable a UI Widget based on its String Identifier.
        /// </summary>
        public static void DisableUIWidget(string _uiWidgetStringIdentifier) {
            if (string.IsNullOrEmpty(_uiWidgetStringIdentifier)) { //If the String Identifier is null or empty
                Logger.TraceError("UI Widget System", "Unable to disable a UIWidget with a null or empty string identifier!");
                return;
            }

            for (int i=0; i<widgetsInstance.Count; i++) { //Loop into every UIWidgets instantiated (RuntimeUIWidget)
                if (widgetsInstance[i].StringIdentifier == _uiWidgetStringIdentifier) { //If it's a match
                    if (widgetsInstance[i].Instance == null) { //Check if there's an instance
                        widgetsInstance.RemoveAt(i); //Remove this element from the list
                        return; //Just stop here
                    }
                    
                    widgetsInstance[i].DisableUIWidget(); //Disable it
                    onWidgetDisable?.Invoke(widgetsInstance[i].Identifier); //Trigger this event
                    
                    return;
                }
            }
            
            Logger.Trace("UI Widget System", $"The UIWidget '{_uiWidgetStringIdentifier}' isn't instantiated!");
        }

        /// <summary>
        /// Disable a UI Widget based in its Identifier.
        /// </summary>
        public static void DisableUIWidget(E_UI_WIDGET_TYPE _uiWidgetIdentifier) {
            if (_uiWidgetIdentifier == E_UI_WIDGET_TYPE.UNKNOWN) { //If the identifier used is not allowed
                Logger.TraceError("UI Widget System", "Unable to disable a UIWidget with the 'UNKNOWN' identifier!");
                return;
            }
            
            for (int i=0; i<widgetsInstance.Count; i++) { //Loop into every UIWidgets instantiated (RuntimeUIWidget)
                if (widgetsInstance[i].Identifier == _uiWidgetIdentifier) { //If it's a match
                    if (widgetsInstance[i].Instance == null) { //Check if there's an instance
                        widgetsInstance.RemoveAt(i); //Remove this element from the list
                        return; //Just stop here
                    }
                    
                    widgetsInstance[i].DisableUIWidget(); //Disable it
                    onWidgetDisable?.Invoke(widgetsInstance[i].Identifier); //Trigger this event

                    return;
                }
            }
            
            Logger.Trace("UI Widget System", $"The UIWidget '{_uiWidgetIdentifier}' isn't instantiated!");
        }
        
        /// <summary>
        /// Enable a UI Widget based on its String Identifier.
        /// </summary>
        public static void EnableUIWidget(string _uiWidgetStringIdentifier) {
            if (string.IsNullOrEmpty(_uiWidgetStringIdentifier)) { //If the String Identifier is null or empty
                Logger.TraceError("UI Widget System", "Unable to enable a UIWidget with a null or empty string identifier!");
                return;
            }

            for (int i=0; i<widgetsInstance.Count; i++) { //Loop into every UIWidgets instantiated (RuntimeUIWidget)
                if (widgetsInstance[i].StringIdentifier == _uiWidgetStringIdentifier) { //If it's a match
                    if (widgetsInstance[i].Instance == null) { //Check if there's an instance
                        widgetsInstance.RemoveAt(i); //Remove this element from the list
                        break; //Break, a new instance will be created
                    }
                    
                    widgetsInstance[i].EnableUIWidget(); //Enable it
                    onWidgetEnabled?.Invoke(widgetsInstance[i].Identifier); //Trigger this event
                    
                    return;
                }
            }

            for (int i = 0; i < widgets.Count; i++) {
                if (widgets[i].IdentifierString == _uiWidgetStringIdentifier) {
                    widgetsInstance.Add(new RuntimeUIWidget(widgets[i], false)); //Create a new RuntimeUIWidget instance, and register it
                    onWidgetEnabled?.Invoke(widgets[i].Identifier); //Trigger this event
                    
                    Logger.Trace("UI Widget System", $"The UIWidget '{_uiWidgetStringIdentifier}' wasn't instantiated, not it is.");
                    return;
                }
            }

            Logger.Trace("UI Widget System", $"The UIWidget '{_uiWidgetStringIdentifier}' isn't instantiated and couldn't be found among the UIWidgetDefinition references!");
        }
        
        /// <summary>
        /// Enable a UI Widget based in its Identifier.
        /// </summary>
        public static void EnableUIWidget(E_UI_WIDGET_TYPE _uiWidgetIdentifier) {
            if (_uiWidgetIdentifier == E_UI_WIDGET_TYPE.UNKNOWN) { //If the identifier used is not allowed
                Logger.TraceError("UI Widget System", "Unable to enable a UIWidget with the 'UNKNOWN' identifier!");
                return;
            }
            
            for (int i=0; i<widgetsInstance.Count; i++) { //Loop into every UIWidgets instantiated (RuntimeUIWidget)
                if (widgetsInstance[i].Identifier == _uiWidgetIdentifier) { //If it's a match
                    if (widgetsInstance[i].Instance == null) { //Check if there's an instance
                        widgetsInstance.RemoveAt(i); //Remove this element from the list
                        break; //Break, a new instance will be created
                    }
                    
                    widgetsInstance[i].EnableUIWidget(); //Enable it
                    onWidgetEnabled?.Invoke(widgetsInstance[i].Identifier); //Trigger this event

                    return;
                }
            }
            
            for (int i = 0; i < widgets.Count; i++) {
                if (widgets[i].Identifier == _uiWidgetIdentifier) {
                    widgetsInstance.Add(new RuntimeUIWidget(widgets[i], false)); //Create a new RuntimeUIWidget instance, and register it
                    onWidgetEnabled?.Invoke(widgets[i].Identifier); //Trigger this event
                    
                    Logger.Trace("UI Widget System", $"The UIWidget '{_uiWidgetIdentifier}' wasn't instantiated, now it is.");
                    return;
                }
            }

            Logger.Trace("UI Widget System", $"The UIWidget '{_uiWidgetIdentifier}' isn't instantiated and couldn't be found among the UIWidgetDefinition references!");
        }

        /// <summary>
        /// Will reload the entire content registered here.
        /// Warning: Calling this method will destroy every UIWidgets instantiated and running!
        /// This might break the game during Runtime!
        /// </summary>
        public static void ReloadWidgets() {
            foreach (RuntimeUIWidget _widget in widgetsInstance) {
                Object.Destroy(_widget.Instance); //Destroy the instance
            }

            widgets.Clear(); //Clear the list of UIWidgetDefinitions found
            widgetsInstance.Clear(); //Clear the list of RuntimeUIWidgets

            LoadUIWidgetReferencesInternal(); //Reload the UIWidgets
        }
        
        #endregion
        
    }
    
}