using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Oikos.Core {
    
    /// <summary>
    /// The Log type
    /// </summary>
    internal enum E_LOG_TYPE {
        TRACE = 0,
        WARNING,
        ERROR,
        EXCEPTION
    }
    
    /// <summary>
    /// This custom Logger allows the messages (Debug.Log/Warning/Error/...) to be stylized in the editor's console,
    /// and log files to be written directly from a build in a more readable way than the engine can offer.
    ///
    /// The PreProcessor Definition "LOGGER_ENABLE" must be active for this logger to work, and "LOGGER_ENABLE_FILE" for the Logger
    /// to write Log Files in a Standalone build.
    /// </summary>
    public static class Logger {

        #region Attributes

        /// <summary>
        /// The color of the trace Debug.Log messages inside the Editor.
        /// </summary>
        private const string TRACE_LOG_COLOR = nameof(Color.white);
        /// <summary>
        /// The color of the trace Debug.LogWarning inside the Editor.
        /// </summary>
        private const string WARNING_LOG_COLOR = nameof(Color.yellow);
        /// <summary>
        /// The color of the trace Debug.LogError inside the Editor.
        /// </summary>
        private const string ERROR_LOG_COLOR = nameof(Color.red);
        
        /// <summary>
        /// The StreamWriter of the current log file.
        /// </summary>
        private static StreamWriter logWriter = null;
        /// <summary>
        /// The FileStream of the current log file.
        /// </summary>
        private static FileStream logFileStream = null;
        /// <summary>
        /// Is the logger able to write logs inside a file.
        /// </summary>
        private static bool allowWriteLog = false;
        
        #endregion

        #region Logger's Init Methods

        /// <summary>
        /// Initialize the Logger's required content
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] private static void InitializeLogger() {
#if !UNITY_EDITOR //Don't write a log file when inside the Editor
            TrySetupLogFile();
            
            Application.quitting += () => { logWriter?.Close(); };
#endif
        }

        /// <summary>
        /// If allowed to, setup the file to receive the log calls.
        /// </summary>
        [Conditional("LOGGER_ENABLE_FILE")]
        private static void TrySetupLogFile() {
            string _logFilePath = $"{Application.dataPath}/Log_Latest.txt"; //The Log file's path
            FileInfo _logFileInfo = new FileInfo(_logFilePath); //Set the FileInfo
            DirectoryInfo _logDirectoryInfo = null;
            
            if(_logFileInfo.DirectoryName != null) {
                _logDirectoryInfo = new DirectoryInfo(_logFileInfo.DirectoryName);
            } else {
                TraceError("Logger", "Unable to write logs into a file!");
                return;
            }
            
            if(!_logDirectoryInfo.Exists) _logDirectoryInfo.Create(); //If the directory doesn't exists, create it
            logFileStream = !_logFileInfo.Exists ? _logFileInfo.Create() :  new FileStream(_logFilePath, FileMode.CreateNew); //If the log file doesn't exists, create it. Otherwise, erase it.
            
            allowWriteLog = true;
            logWriter = new StreamWriter(logFileStream);
            
            //Write a basic Log setup
            logWriter.WriteLine($"{Application.productName} - {Application.companyName}  ({Application.version} - {Application.platform})");
            logWriter.WriteLine($"Output log file ({DateTime.Now.ToString(new CultureInfo("fr-fr"))})");
            logWriter.WriteLine($"-------------------------------------------------------------------------------------------------------------------------");
        }
        
        #endregion

        #region Logger's Trace Methods

        [Conditional("LOGGER_ENABLE")]
        public static void Trace(object _message) {
            Debug.Log(FormatMessage(TRACE_LOG_COLOR, _message));
            WriteOnFile(E_LOG_TYPE.TRACE, _message.ToString());
        }

        [Conditional("LOGGER_ENABLE")]
        public static void Trace(string _category, object _message) {
            Debug.Log(FormatMessageWithCategory(TRACE_LOG_COLOR, _category, _message));
            WriteOnFile(E_LOG_TYPE.TRACE, _message.ToString(), _category);
        }

        [Conditional("LOGGER_ENABLE")]
        public static void TraceFormat(string _format, params object[] _args) {
            Debug.Log(FormatMessage(TRACE_LOG_COLOR, string.Format(_format, _args)));
            WriteOnFile(E_LOG_TYPE.TRACE, string.Format(_format, _args));
        }

        [Conditional("LOGGER_ENABLE")]
        public static void TraceFormat(string _category, string _format, params object[] _args) {
            Debug.Log(FormatMessageWithCategory(TRACE_LOG_COLOR, _category, string.Format(_format, _args)));
            WriteOnFile(E_LOG_TYPE.TRACE, string.Format(_format, _args), _category);
        }

        [Conditional("LOGGER_ENABLE")]
        public static void TraceWarning(object _message) {
            Debug.LogWarning(FormatMessage(WARNING_LOG_COLOR, _message));
            WriteOnFile(E_LOG_TYPE.WARNING, _message.ToString());
        }

        [Conditional("LOGGER_ENABLE")]
        public static void TraceWarning(string _category, object _message) {
            Debug.LogWarning(FormatMessageWithCategory(WARNING_LOG_COLOR, _category, _message));
            WriteOnFile(E_LOG_TYPE.WARNING, _message.ToString());
        }

        [Conditional("LOGGER_ENABLE")]
        public static void TraceWarningFormat(string _format, params object[] _args) {
            Debug.LogWarningFormat(FormatMessage(WARNING_LOG_COLOR, string.Format(_format, _args)));
            WriteOnFile(E_LOG_TYPE.WARNING, string.Format(_format, _args));
        }

        [Conditional("LOGGER_ENABLE")]
        public static void TraceWarningFormat(string _category, string _format, params object[] _args) {
            Debug.LogWarningFormat(FormatMessageWithCategory(WARNING_LOG_COLOR, _category, string.Format(_format, _args)));
            WriteOnFile(E_LOG_TYPE.WARNING, string.Format(_format, _args), _category);
        }

        [Conditional("LOGGER_ENABLE")]
        public static void TraceError(object _message) {
            Debug.LogError(FormatMessage(ERROR_LOG_COLOR, _message));
            WriteOnFile(E_LOG_TYPE.ERROR, _message.ToString());
        }

        [Conditional("LOGGER_ENABLE")]
        public static void TraceError(string _category, object _message) {
            Debug.LogError(FormatMessageWithCategory(ERROR_LOG_COLOR, _category, _message));
            WriteOnFile(E_LOG_TYPE.ERROR, _message.ToString(), _category);
        }

        [Conditional("LOGGER_ENABLE")]
        public static void TraceErrorFormat(string _format, params object[] _args) {
            Debug.LogErrorFormat(FormatMessage(ERROR_LOG_COLOR, string.Format(_format, _args)));
            WriteOnFile(E_LOG_TYPE.ERROR, string.Format(_format, _args));
        }

        [Conditional("LOGGER_ENABLE")]
        public static void TraceErrorFormat(string _category, string _format, params object[] _args) {
            Debug.LogErrorFormat(FormatMessageWithCategory(ERROR_LOG_COLOR, _category, string.Format(_format, _args)));
            WriteOnFile(E_LOG_TYPE.ERROR, string.Format(_format, _args), _category);
        }

        [Conditional("LOGGER_ENABLE")]
        public static void TraceException(Exception _exception) {
            Debug.LogError(FormatMessage(ERROR_LOG_COLOR, _exception.Message));
        }

        [Conditional("LOGGER_ENABLE")]
        public static void TraceException(string _category, Exception _exception) {
            Debug.LogError(FormatMessageWithCategory(ERROR_LOG_COLOR, _category, _exception.Message));
        }

        #endregion
        
        #region Logger's Format Message Methods

        /// <summary>
        /// Format a message to use the correct color
        /// </summary>
        /// <param name="_color">The color of this message</param>
        /// <param name="_message">The message</param>
        private static string FormatMessage(string _color, object _message) {
            return $"<color={_color}>{_message}</color>";
        }
        
        /// <summary>
        /// Format a message to use the correct color and category
        /// </summary>
        /// <param name="_color">The color of this message</param>
        /// <param name="_message">The message</param>
        /// <param name="_category">The category of this message</param>
        private static string FormatMessageWithCategory(string _color, string _category, object _message) {
            return $"<color={_color}><b>[{_category}]</b> {_message}</color>";
        }

        #endregion
        
        #region Logger's Write Methods

        /// <summary>
        /// Write a log on the current LogFile used.
        /// </summary>
        /// <param name="_logType">The Log Type</param>
        /// <param name="_message">The Log message</param>
        /// <param name="_category">The Log Category</param>
        [Conditional("LOGGER_ENABLE_FILE")]
        private static void WriteOnFile(E_LOG_TYPE _logType, string _message, string _category = "- - -") {
            if(!allowWriteLog) return; //Don't write log calls into the file if not allowed
            
                                //00-00-2022 00:00:00                                            TRACE         CATEGORY     MESSAGE
            logWriter.WriteLine($"[{DateTime.Now.ToString(new CultureInfo("fr-fr"))}] [{_logType}] [{_category}] {_message}");
        }

        #endregion
        
    }
    
}