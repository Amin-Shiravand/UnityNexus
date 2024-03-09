using NUnit.Framework;
using Nexus.Encryption;
using System;

public class AesEncryptionServiceTests
{
    private string testPassword = "TestPassword";
    private string testSalt = "TestSalt"; 
    private int keySize = 256; 

    [Test]
    public void EncryptDecrypt_ReturnsOriginalMessage()
    {
        string originalMessage = "Hello, Unity Encryption Tests!";
        var aesService =  EncryptionServiceFactory.CreateEncryptionService(testPassword, testSalt, keySize);

        string encryptedMessage = aesService.Encrypt(originalMessage);
        string decryptedMessage = aesService.Decrypt(encryptedMessage);

        Assert.AreEqual(originalMessage, decryptedMessage, "Decrypted message should match the original message.");
    }

    [Test]
    public void Encrypt_SameDataDifferentOutputs()
    {
        string message = "Repeated encryption test";
        var aesService = EncryptionServiceFactory.CreateEncryptionService(testPassword, testSalt, keySize);

        string encryptedFirst = aesService.Encrypt(message);
        string encryptedSecond = aesService.Encrypt(message);

        Assert.AreNotEqual(encryptedFirst, encryptedSecond, "Encrypting the same data should produce different outputs due to IV.");
    }

    [Test]
    public void Decrypt_WithWrongKey_ThrowsException()
    {
        string originalMessage = "Decryption with wrong key test";
        var aesService =  EncryptionServiceFactory.CreateEncryptionService(testPassword, testSalt, keySize);
        string encryptedMessage = aesService.Encrypt(originalMessage);

        var aesServiceWrongKey = new AesEncryptionService("WrongPassword", testSalt, keySize);

        Assert.Throws<Exception>(() => aesServiceWrongKey.Decrypt(encryptedMessage), "Decryption with a wrong key should fail.");
    }

    [Test]
    public void Constructor_InvalidKeySize_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>  EncryptionServiceFactory.CreateEncryptionService(testPassword, testSalt, 123), "Constructor should throw ArgumentException for invalid key sizes.");
    }
}
