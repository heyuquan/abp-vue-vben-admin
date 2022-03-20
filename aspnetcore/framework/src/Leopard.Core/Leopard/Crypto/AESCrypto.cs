
using System;
using System.Buffers.Text;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Leopard.Crypto;

/// <summary>
/// AES 加解密
/// （使用 CryptoGuide 静态类进行访问）
/// </summary>
public class AESCrypto
{

    /// <summary>
    /// 使用 CryptoGuide 静态类进行访问
    /// </summary>
    internal AESCrypto() { }

    /// <summary>
    /// 使用用户口令，生成符合AES标准的key和iv。
    /// </summary>
    /// <param name="password">用户输入的口令</param>
    /// <returns>返回包含utf8密钥和utf8向量的元组</returns>
    public (string Key, string IV) GenerateKeyAndIV(string password)
    {
        byte[] key = new byte[32];
        byte[] iv = new byte[16];
        byte[] hash = default;
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("必须输入口令！");
        using (SHA384 sha = SHA384.Create())
        {
            byte[] buffer = Encoding.UTF8.GetBytes(password);
            hash = sha.ComputeHash(buffer);
        }
        //用SHA384的原因：生成的384位哈希值正好被分成两段使用。(32+16)*8=384。
        Array.Copy(hash, 0, key, 0, 32);//生成256位密钥（32*8=256）
        Array.Copy(hash, 32, iv, 0, 16);//生成128位向量（16*8=128）

        return (Key: Encoding.UTF8.GetString(key), IV: Encoding.UTF8.GetString(iv));
    }


    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="encodeType">明文内容</param>
    /// <param name="plaintext">对plaintext进行编码采用的编码方式</param>
    /// <param name="key_utf8">key</param>
    /// <param name="iv_utf8">IV</param>
    /// <returns>密文</returns>
    public string EncryptWithEncode(Encoding encodeType, string plaintext, string key_utf8, string iv_utf8)
    {
        byte[] inputBytes = encodeType.GetBytes(plaintext);
        var encryptByte = Encrypt(inputBytes, key_utf8, iv_utf8);

        return encodeType.GetString(encryptByte);
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="source">明文内容Byte数组</param>
    /// <param name="key_utf8">key</param>
    /// <param name="iv_utf8">IV</param>
    /// <returns>密文</returns>
    public byte[] Encrypt(byte[] source, string key_utf8, string iv_utf8)
    {
        byte[] keyBytes = UTF8Encoding.UTF8.GetBytes(key_utf8);
        byte[] ivBytes = UTF8Encoding.UTF8.GetBytes(iv_utf8);

        //创建加密器对象（加解密方法不同处仅仅这一句话）
        var encryptor = GetCryptoAlgorithm().CreateEncryptor(keyBytes, ivBytes);

        byte[] encrypted;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(source, 0, source.Length);   //对原数组加密并写入流中
                cryptoStream.FlushFinalBlock();

                encrypted = memoryStream.ToArray();
            }
        }

        return encrypted;
    }


    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="encodeType">对cipherText进行解码采用的解码方式，注意和编码时采用的方式一致</param>
    /// <param name="cipherText">key</param>
    /// <param name="key_utf8">key</param>
    /// <param name="iv_utf8">IV</param>
    /// <returns>明文</returns>
    public string DecryptWithEncode(Encoding encodeType, string cipherText, string key_utf8, string iv_utf8)
    {
        byte[] inputBytes = encodeType.GetBytes(cipherText);
        var plaintext = Decrypt(inputBytes, key_utf8, iv_utf8);

        return encodeType.GetString(plaintext);
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="source">待解密的byte数组</param>
    /// <param name="key_utf8">key</param>
    /// <param name="iv_utf8">IV</param>
    /// <returns>明文</returns>
    public byte[] Decrypt(Byte[] source, string key_utf8, string iv_utf8)
    {
        byte[] keyBytes = UTF8Encoding.UTF8.GetBytes(key_utf8);
        byte[] ivBytes = UTF8Encoding.UTF8.GetBytes(iv_utf8);

        //创建解密器对象（加解密方法不同处仅仅这一句话）
        var decryptor = GetCryptoAlgorithm().CreateDecryptor(keyBytes, ivBytes);

        byte[] decrypted;
        using (MemoryStream memoryStream = new MemoryStream(source))
        {
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                byte[] buffer_T = new byte[source.Length];/*--s1:创建临时数组，用于包含可用字节+无用字节--*/

                int i = cryptoStream.Read(buffer_T, 0, source.Length);/*--s2:对加密数组进行解密，并通过i确定实际多少字节可用--*/

                //csDecrypt.FlushFinalBlock();//使用Read模式不能有此句，但write模式必须要有。

                decrypted = new byte[i];/*--s3:创建只容纳可用字节的数组--*/

                Array.Copy(buffer_T, 0, decrypted, 0, i);/*--s4:从bufferT拷贝出可用字节到decrypted--*/
            }
        }
        return decrypted;
    }


    private RijndaelManaged GetCryptoAlgorithm()
    {
        RijndaelManaged algorithm = new RijndaelManaged();
        //set the mode, padding and block size
        algorithm.Padding = PaddingMode.PKCS7;
        algorithm.Mode = CipherMode.CBC;
        // KeySize 默认 256
        algorithm.KeySize = 256;
        algorithm.BlockSize = 128;
        return algorithm;
    }
}
