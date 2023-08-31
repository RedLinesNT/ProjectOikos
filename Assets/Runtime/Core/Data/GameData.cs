using UnityEngine;

namespace Oikos.Core {
    
    /// <summary>
    /// Contains data intended for use by game systems and Gameplay components
    /// </summary>
    public abstract class GameData : ScriptableObject {}
    
    /// <summary>
    /// Contains data intended for use by game systems and Gameplay components
    /// </summary>
    public abstract class GameData<T> : GameData where T : struct {

        #region Properties

        public abstract T Data { get; }

        #endregion
        
    }
    
}