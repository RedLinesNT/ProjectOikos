using UnityEngine;

namespace Oikos.Core {
    
    /// <summary>
    /// Contains data for internal use with the engine and systems designed for this project.
    /// </summary>
    public abstract class EngineData : ScriptableObject {}
    
    /// <summary>
    /// Contains data for internal use with the engine and systems designed for this project.
    /// </summary>
    public abstract class EngineData<T> : EngineData where T : struct {

        #region Properties

        public abstract T Data { get; }

        #endregion
        
    }
    
}