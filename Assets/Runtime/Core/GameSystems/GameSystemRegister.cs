using System.Collections.Generic;
using System;
using Oikos.GameLogic.Systems;

namespace Oikos.Core.Systems {
    
    public static class GameSystemRegister {
        
        private static readonly Dictionary<E_GAME_SYSTEM_TYPE, Type> gameSystemRegister = new Dictionary<E_GAME_SYSTEM_TYPE, Type>() {
            { E_GAME_SYSTEM_TYPE.TRASH_OBJECT_SPAWNER, typeof(TrashObjectManagerSystem) }, //Trash Object Manager System
        };
        
        /// <summary>
        /// Return a GameSystem from the type asked.
        /// </summary>
        /// <param name="_gameSysType">The GameSystem type asked</param>
        public static Type FindGameSystemFromType(E_GAME_SYSTEM_TYPE _gameSysType) {
            return gameSystemRegister[_gameSysType];
        }

    }
    
}