using System;

namespace JazzyLucas.Utils
{
    /// <summary>
    /// Token Utility methods
    /// </summary>
    public static class TokenUtils
    {
        private static bool hasGeneratedToken; 
        private static byte[] generatedToken;
        
        /// <summary>
        /// Create new random Token
        /// </summary>
        public static byte[] NewToken(bool singleton)
        {
            switch (singleton)
            {
                case true when !hasGeneratedToken:
                case false:
                    generatedToken = Guid.NewGuid().ToByteArray();
                    break;
            }
            hasGeneratedToken = true;
            return generatedToken;
        } 

        /// <summary>
        /// Converts a Token into a Hash format
        /// </summary>
        /// <param name="token">Token to be hashed</param>
        /// <returns>Token hash</returns>
        public static int HashToken(byte[] token) => new Guid(token).GetHashCode();
    }
}