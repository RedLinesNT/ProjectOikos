using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Oikos.Core.Platform.Windows {
    
    /// <summary>
    /// Contain methods for displaying alerts (MessageBox) on Windows Platform
    /// </summary>
    public static class Alert {

        #region Extern DLL Imports

        [DllImport("user32.dll")] private static extern IntPtr GetActiveWindow();
        [DllImport("user32.dll", SetLastError = true)] private static extern int MessageBox(IntPtr _hwnd, String _lpContent, String _lpTitle, uint _uType);

        #endregion

        #region Alert's methods

        /// <summary>
        /// Display a Fatal Error alert box on Windows, then close the application
        /// </summary>
        /// <param name="_title">The title of the message box</param>
        /// <param name="_content">The content of the message box</param>
        public static void FatalError(string _title, string _content) {
            try {
                MessageBox(GetActiveWindow(), $"{_content}.\nThe process must be terminated.", $"FATAL ERROR: {_title}", (uint)(0x00000000L | 0x00000010L));
            } catch { /* Ignored */ }

            Application.Quit(-1); //Close the application
        }

        #endregion
        
    }
    
}