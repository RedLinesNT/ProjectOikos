﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Oikos.Core.UI {

    public static class UIWidgetSystem {

        #region Runtime values

        /// <summary>
        /// List of every "UIWidgetDefinition" referenced inside "UIWidgetReferences".
        /// </summary>
        private static List<UIWidgetDefinition> widgets = new List<UIWidgetDefinition>();
        /// <summary>
        /// List of every instantiated UIWidgets
        /// </summary>
        private static List<RuntimeUIWidget> widgetsInstance = new List<RuntimeUIWidget>();

        #endregion

        #region UIWidgetSystem's internal methods

        /// <summary>
        /// Called at the launch of the game.
        /// Used to load the required content.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)] private static void Initialize() {
            LoadUIWidgetReferencesInternal();
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
                    widgetsInstance[i].DisableUIWidget(); //Disable it

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
                    widgetsInstance[i].DisableUIWidget(); //Disable it

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
                    widgetsInstance[i].EnableUIWidget(); //Enable it

                    return;
                }
            }

            for (int i = 0; i < widgets.Count; i++) {
                if (widgets[i].IdentifierString == _uiWidgetStringIdentifier) {
                    widgetsInstance.Add(new RuntimeUIWidget(widgets[i], false)); //Create a new RuntimeUIWidget instance, and register it
                    
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
                    widgetsInstance[i].EnableUIWidget(); //Enable it
                }
            }
            
            for (int i = 0; i < widgets.Count; i++) {
                if (widgets[i].Identifier == _uiWidgetIdentifier) {
                    widgetsInstance.Add(new RuntimeUIWidget(widgets[i], false)); //Create a new RuntimeUIWidget instance, and register it

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