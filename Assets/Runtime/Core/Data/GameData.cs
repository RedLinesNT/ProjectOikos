using UnityEngine;

namespace Oikos.Core {
    
    public abstract class GameData : ScriptableObject {}
    
    public abstract class GameData<T> : GameData where T : struct {

        #region Properties

        public abstract T Data { get; }

        #endregion
        
    }
    
}