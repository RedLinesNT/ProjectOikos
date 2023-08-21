using UnityEngine;

namespace Oikos.Core {
    
    /// <summary>
    /// Contain methods for displaying MessageBox on the correct platform (Windows, OSX, Android, ...)
    /// </summary>
    public static class MessageBox {
        
        /// <summary>
        /// Display a Fatal Error MessageBox on the current platform, then close the Application.
        /// </summary>
        /// <param name="_title">The title of the message box</param>
        /// <param name="_content">The content of the message box</param>
        public static void FatalError(string _title, string _content) {
            Logger.TraceError($"Fatal Error: {_title}", $"{_content}. The application must be closed!"); //Trace the error

#if UNITY_EDITOR //Stop the play mode in the Editor
            UnityEditor.EditorApplication.isPlaying = false;
#endif

#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            Platform.Windows.Alert.FatalError(_title, _content);
#else
            //In unsupported platforms
            Utils.ForceCrash(ForcedCrashCategory.FatalError);
#endif
            
        }
        
    }
    
}