using Leopard.Utils;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Leopard.Crypto;

/// <summary>
/// RSA 加密
/// （使用 CryptoGuide 静态类进行访问）
/// </summary>
public class RSACrypto : Singleton<RSACrypto>
{
    static RSACrypto()
    {
        Instance = new RSACrypto();
    }

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
    /// 使用公钥加密
    /// </summary>
    /// <param name="encodeType">明文内容</param>
    /// <param name="plaintext">对plaintext进行编码采用的编码方式</param>
    /// <param name="publicKey">公钥</param>
    /// <param name="keySize"></param>
    /// <returns></returns>
    public string EncryptWithEncodeByPublicKey(Encoding encodeType, string plaintext, string publicKey, int keySize = 2048)
    {
        byte[] inputBytes = encodeType.GetBytes(plaintext);
        var encryptByte = EncryptByPublicKey(inputBytes, publicKey, keySize);

        return encodeType.GetString(encryptByte);
    }

    /// <summary>
    /// 使用公钥加密
    /// </summary>
    /// <param name="plainByte">明文内容</param>
    /// <param name="publicKey">公钥</param>
    /// <param name="keySize"></param>
    /// <returns></returns>
    public byte[] EncryptByPublicKey(byte[] plainByte, string publicKey, int keySize = 2048)
    {
        CheckRSAKeySize(keySize);

        using var rsa = new RSACryptoServiceProvider(keySize);
        rsa.FromXmlString(publicKey);

        var encryptedData = rsa.Encrypt(plainByte, false);
        return encryptedData;
    }

    /// <summary>
    /// 使用私钥解密
    /// </summary>
    /// <param name="encodeType">对cipherText进行解码采用的解码方式，注意和编码时采用的方式一致</param>
    /// <param name="cipherText">key</param>
    /// <param name="privateKey">私钥</param>
    /// <param name="keySize"></param>
    /// <returns></returns>
    public string DecryptWithEncodeByPrivateKey(Encoding encodeType, string cipherText, string privateKey, int keySize = 2048)
    {
        byte[] inputBytes = encodeType.GetBytes(cipherText);
        var plaintext = DecryptByPrivateKey(inputBytes, privateKey, keySize);

        return encodeType.GetString(plaintext);
    }

    /// <summary>
    /// 使用私钥解密
    /// </summary>
    /// <param name="cipherByte">密文内容</param>
    /// <param name="privateKey">私钥</param>
    /// <param name="keySize"></param>
    /// <returns></returns>
    public byte[] DecryptByPrivateKey(byte[] cipherByte, string privateKey, int keySize = 2048)
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

    /// <summary>
    /// 私钥 生成数字签名（服务端向客户端提供签名）
    /// </summary>
    /// <param name="originalText">原文</param>
    /// <param name="privateKey"></param>
    /// <returns></returns>
    public static string GenSign(string originalText, string privateKey)
    {
        byte[] byteData = Encoding.UTF8.GetBytes(originalText);
        RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
        provider.FromXmlString(privateKey);
        //使用SHA1进行摘要算法，生成签名
        byteData = provider.SignData(byteData, new SHA1CryptoServiceProvider());
        return CryptoGuide.Base64.Encode(byteData);
    }

    /// <summary>
    /// 公钥 验证签名
    /// </summary>
    /// <param name="originalText">原文</param>
    /// <param name="signedData">签名</param>
    /// <param name="publicKey">公钥</param>
    /// <returns></returns>
    public static bool VerifySigned(string originalText, string signedData, string publicKey)
    {
        RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
        provider.FromXmlString(publicKey);
        byte[] byteData = Encoding.UTF8.GetBytes(originalText);
        byte[] signData = CryptoGuide.Base64.DecodeToByte(signedData);
        return provider.VerifyData(byteData, new SHA1CryptoServiceProvider(), signData);
    }

    // https://blog.csdn.net/qq_37835111/article/details/87358779
    // 这里提供私钥加密，有需要再研究
    ///// <summary>
    ///// 私钥加密
    ///// </summary>
    ///// <param name="s">明文</param>
    ///// <param name="key">私钥(java产出的string)</param>
    //public static string EncryptByPrivateKey(string s, string key)
    //{
    //    //非对称加密算法，加解密用  
    //    IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

    //    //加密  
    //    try
    //    {
    //        engine.Init(true, GetPrivateKeyParameter(key));
    //        byte[] byteData = System.Text.Encoding.UTF8.GetBytes(s);
    //        var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
    //        return Convert.ToBase64String(ResultData);
    //        //Console.WriteLine("密文（base64编码）:" + Convert.ToBase64String(testData) + Environment.NewLine);
    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.Message;
    //    }
    //}

    ///// <summary>
    ///// 公钥解密
    ///// </summary>
    ///// <param name="s">明文</param>
    ///// <param name="key">公钥(java产出的string)</param>
    //public static string DecryptByPublicKey(string s, string key)
    //{
    //    s = s.Replace("\r", "").Replace("\n", "").Replace(" ", "");
    //    //非对称加密算法，加解密用  
    //    IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

    //    //解密  
    //    try
    //    {
    //        engine.Init(false, GetPublicKeyParameter(key));
    //        byte[] byteData = Convert.FromBase64String(s);
    //        var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
    //        return System.Text.Encoding.UTF8.GetString(ResultData);
    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.Message;
    //    }
    //}

}
