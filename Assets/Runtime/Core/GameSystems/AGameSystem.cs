using System;

namespace Oikos.Core.Systems {
    
    public abstract class AGameSystem {

        #region Properties

        /// <summary>
        /// Is this GameSystem initialized
        /// </summary>
        public static bool IsInitialized { get; private protected set; } = false;

        /// <summary>
        /// The internal name of this GameSystem
        /// </summary>
        public static string InternalName { get; private protected set; } = string.Empty;
        
        #endregion
        
        #region AGameSystem's virtual methods

        /// <summary>
        /// InitializeSystem is called when this GameSystem is instantiated
        /// </summary>
        public virtual void InitializeSystem(){}

        /// <summary>
        /// DisposeSystem is called when this GameSystem should shutdown
        /// </summary>
        public virtual void DisposeSystem(){}
        
        /// <summary>
        /// ReloadSystem is called when this GameSystem should reload
        /// </summary>
        public virtual void ReloadSystem(){}
        
        #endregion
        
    }
    
}