using System;

namespace Oikos.Core {
    
    /// <summary>
    /// Contains utility methods to facilitate repetitive calculations
    /// </summary>
    public static class Utils {
        
        /// <summary>
        /// Returns a generated Universally Unique Identifier (UUID) as a string
        /// </summary>
        public static string GenerateUUID() {
            return Guid.NewGuid().ToString();
        }
        
    }
    
}