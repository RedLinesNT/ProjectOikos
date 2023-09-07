using System;
using System.Collections.Generic;

namespace Oikos.Core.Systems {
    
    public static class GameSystemModule {

        #region Attributes

        /// <summary>
        /// The list of every GameSystem currently loaded
        /// </summary>
        private static Dictionary<E_GAME_SYSTEM_TYPE, AGameSystem> loadedGameSystem = new Dictionary<E_GAME_SYSTEM_TYPE, AGameSystem>();

        #endregion
        
        
        /// <summary>
        /// Return a GameSystem from the type asked.
        /// </summary>
        /// <param name="_gameSysType">The GameSystem type asked</param>
        private static AGameSystem FindGameSystemFromType(E_GAME_SYSTEM_TYPE _gameSysType) {
            Dictionary<E_GAME_SYSTEM_TYPE, AGameSystem> _gameSystemList = new Dictionary<E_GAME_SYSTEM_TYPE, AGameSystem>() {
                { E_GAME_SYSTEM_TYPE.TRASH_OBJECT_SPAWNER, null }, //Trash Spawner System
            };
            
            return _gameSystemList[_gameSysType];
        }
        
    }
    
}