using UnityEngine;

namespace Oikos.Core {
    
    public abstract class EngineData : ScriptableObject {}
    
    public abstract class EngineData<T> : EngineData where T : struct {

        #region Properties

        public abstract T Data { get; }

        #endregion
        
    }
    
}