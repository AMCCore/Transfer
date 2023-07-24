﻿using System.Security.Cryptography;
using System.Text;

namespace Transfer.Common.Utils;

public static class EncryptionHelper
{
    private const string key = "FKGuxT8CQ#ykvPVktiyA9WJrD7RVf&F3UuavN&wD";

    public static string EncryptString(string plainText)
    {
        byte[] iv = new byte[16];
        byte[] array;

        using var aes = Aes.Create();

        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = iv;

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using var streamWriter = new StreamWriter(cryptoStream);
            
        streamWriter.Write(plainText);
            
        array = memoryStream.ToArray();

        return Convert.ToBase64String(array);
    }

    public static string DecryptString(string cipherText)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = iv;
        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream(buffer);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);

        return streamReader.ReadToEnd();
    }
}
