using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Services;

public static class CryptoHelper
{
    private static readonly string fixedSalt = "abcdefg12345";
    private static readonly byte[] internalKey = Encoding.UTF8.GetBytes("SecureFixedKey1234");

    private const int Rfc2898KeygenIterations = 100;
    private const int AesKeySizeInBits = 128;

    private static Aes CreateAes(byte[] saltBytes)
    {
        var aes = Aes.Create();
        aes.Padding = PaddingMode.PKCS7;
        aes.KeySize = AesKeySizeInBits;
        var keyStrengthInBytes = aes.KeySize / 8;

        using var rfc2898 = new Rfc2898DeriveBytes(internalKey, saltBytes, Rfc2898KeygenIterations, HashAlgorithmName.SHA1);
        aes.Key = rfc2898.GetBytes(keyStrengthInBytes);
        aes.IV = rfc2898.GetBytes(keyStrengthInBytes);

        return aes;
    }

    public static string Encrypt(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var saltBytes = Encoding.UTF8.GetBytes(fixedSalt);

        using var aes = CreateAes(saltBytes);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cs.Write(inputBytes, 0, inputBytes.Length);
            cs.FlushFinalBlock();
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string encrypted)
    {
        var encodedBytes = Convert.FromBase64String(encrypted);
        var saltBytes = Encoding.UTF8.GetBytes(fixedSalt);

        using var aes = CreateAes(saltBytes);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
        {
            cs.Write(encodedBytes, 0, encodedBytes.Length);
            cs.FlushFinalBlock();
        }

        return Encoding.UTF8.GetString(ms.ToArray());
    }
}
