using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Nexus.Encryption
{
    public class AesEncryptionService : IEncryptionService
    {
        private readonly byte[] encryptionKey;
        private readonly byte[] salt;
        private readonly int keySize;

        public AesEncryptionService(string password, string predefinedSalt, int keySize = 256)
        {
            if (keySize != 128 && keySize != 192 && keySize != 256)
            {
                throw new ArgumentException("Invalid key size. Key size must be 128, 192, or 256 bits.", nameof(keySize));
            }

            this.salt = Encoding.UTF8.GetBytes(predefinedSalt);

            this.keySize = keySize;
            this.encryptionKey = GenerateKeyFromPassword(password, keySize / 8);
        }

        private byte[] GenerateKeyFromPassword(string password, int keyBytes = 32)
        {
            using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 10000);
            return rfc2898DeriveBytes.GetBytes(keyBytes);
        }

        public string Encrypt(string plainText)
        {
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = encryptionKey;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.GenerateIV();

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                encrypted = aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray();
            }

            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string cipherText)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);
            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = encryptionKey;
                aesAlg.Mode = CipherMode.CBC;

                byte[] iv = new byte[aesAlg.BlockSize / 8];
                Array.Copy(fullCipher, 0, iv, 0, iv.Length);
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msDecrypt = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length);
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    plaintext = srDecrypt.ReadToEnd();
                }
            }

            return plaintext;
        }
    }
}
