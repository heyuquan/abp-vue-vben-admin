using System;
using System.Security.Cryptography;
using System.Text;

namespace Leopard.Crypto;

/// <summary>
/// RSA 加密
/// （使用 CryptoGuide 静态类进行访问）
/// </summary>
public class RSACrypto
{
    /// <summary>
    /// 使用 CryptoGuide 静态类进行访问
    /// </summary>
    internal RSACrypto() { }

    /// <summary>
    /// 生成 RSA 秘钥
    /// </summary>
    /// <param name="keySize">大小必须为 2048 到 16384 之间，且必须能被 8 整除</param>
    /// <returns></returns>
    public (string publicKey, string privateKey) GenerateSecretKey(int keySize = 2048)
    {
        CheckRSAKeySize(keySize);

        using var rsa = new RSACryptoServiceProvider(keySize);
        return (rsa.ToXmlString(false), rsa.ToXmlString(true));
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="encodeType">明文内容</param>
    /// <param name="plaintext">对plaintext进行编码采用的编码方式</param>
    /// <param name="publicKey">公钥</param>
    /// <param name="keySize"></param>
    /// <returns></returns>
    public string EncryptWithEncode(Encoding encodeType, string plaintext, string publicKey, int keySize = 2048)
    {
        byte[] inputBytes = encodeType.GetBytes(plaintext);
        var encryptByte = Encrypt(inputBytes, publicKey, keySize);

        return encodeType.GetString(encryptByte);
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="plainByte">明文内容</param>
    /// <param name="publicKey">公钥</param>
    /// <param name="keySize"></param>
    /// <returns></returns>
    public byte[] Encrypt(byte[] plainByte, string publicKey, int keySize = 2048)
    {
        CheckRSAKeySize(keySize);

        using var rsa = new RSACryptoServiceProvider(keySize);
        rsa.FromXmlString(publicKey);

        var encryptedData = rsa.Encrypt(plainByte, false);
        return encryptedData;
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="encodeType">对cipherText进行解码采用的解码方式，注意和编码时采用的方式一致</param>
    /// <param name="cipherText">key</param>
    /// <param name="privateKey">私钥</param>
    /// <param name="keySize"></param>
    /// <returns></returns>
    public string DecryptWithEncode(Encoding encodeType, string cipherText, string privateKey, int keySize = 2048)
    {
        byte[] inputBytes = encodeType.GetBytes(cipherText);
        var plaintext = Decrypt(inputBytes, privateKey, keySize);

        return encodeType.GetString(plaintext);
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="cipherByte">密文内容</param>
    /// <param name="privateKey">私钥</param>
    /// <param name="keySize"></param>
    /// <returns></returns>
    public byte[] Decrypt(byte[] cipherByte, string privateKey, int keySize = 2048)
    {
        CheckRSAKeySize(keySize);

        using var rsa = new RSACryptoServiceProvider(keySize);
        rsa.FromXmlString(privateKey);

        var decryptedData = rsa.Decrypt(cipherByte, false);
        return decryptedData;
    }

    /// <summary>
    /// 检查 RSA 长度
    /// </summary>
    /// <param name="keySize"></param>
    private void CheckRSAKeySize(int keySize)
    {
        if (keySize < 2048 || keySize > 16384 || keySize % 8 != 0)
            throw new ArgumentException("The keySize must be between 2048 and 16384 in size and must be divisible by 8.", nameof(keySize));
    }
}
