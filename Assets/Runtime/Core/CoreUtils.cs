using System;

namespace Oikos.Core {
    
    public static class Utils {
        
        /// <summary>
        /// Returns a generated Universally Unique Identifier (UUID) as a string
        /// </summary>
        public static string GenerateUUID() {
            return Guid.NewGuid().ToString();
        }
        
    }
    
}