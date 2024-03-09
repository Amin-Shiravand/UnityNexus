using System;

namespace Nexus.Encryption
{
    public class EncryptionServiceFactory
    {
        public static IEncryptionService CreateEncryptionService(string password, string predefinedSalt, int keySize = 256)
        {
            if( keySize != 128 && keySize != 192 && keySize != 256 )
            {
                throw new ArgumentException("Invalid key size. Key size must be 128, 192, or 256 bits.");
            }

            return new AesEncryptionService(password, predefinedSalt, keySize);
        }
    }
}