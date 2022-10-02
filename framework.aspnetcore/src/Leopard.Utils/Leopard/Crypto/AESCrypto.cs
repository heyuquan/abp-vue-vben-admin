using Leopard.Utils;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Leopard.Crypto;

// https://www.jb51.net/article/199524.htm

/// <summary>
/// AES 加解密
/// （使用 CryptoGuide 静态类进行访问）
/// </summary>
public class AESCrypto : Singleton<AESCrypto>
{
    static AESCrypto()
    {
        Instance = new AESCrypto();
    }

    /// <summary>
    /// 使用用户口令，生成符合AES标准的key和iv。
    /// </summary>
    /// <param name="password">用户输入的口令</param>
    /// <returns>返回包含utf8密钥（256位）和utf8向量（128位）的元组</returns>
    public (byte[] Key, byte[] IV) GenerateKeyAndIV(string password)
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

        return (Key: key, IV: iv);
    }

    /// <summary>
    /// 使用用户口令，生成符合AES标准的key和iv。
    /// </summary>
    /// <param name="password">用户输入的口令</param>
    /// <returns>返回包含utf8密钥和utf8向量的元组</returns>
    public (string Key, string IV) GenerateKeyAndIV_Utf8(string password)
    {
        var keyAndIv = GenerateKeyAndIV(password);
        return (Key: Encoding.UTF8.GetString(keyAndIv.Key), IV: Encoding.UTF8.GetString(keyAndIv.IV));
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

        byte[] encrypted;
        using (Aes aes = Aes.Create())
        {
            //设定密钥和向量
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            //设定运算模式和填充模式
            aes.Mode = CipherMode.CBC;//默认
            aes.Padding = PaddingMode.PKCS7;//默认
            // KeySize 默认 256
            aes.KeySize = 256;
            aes.BlockSize = 128;

            //创建加密器对象
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(source, 0, source.Length);   //对原数组加密并写入流中
                    cryptoStream.FlushFinalBlock();     //使用Read模式不能有此句，但write模式必须要有。

                    encrypted = memoryStream.ToArray();
                }
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

        byte[] decrypted;
        using (Aes aes = Aes.Create())
        {
            //设定密钥和向量
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            //设定运算模式和填充模式
            aes.Mode = CipherMode.CBC;//默认
            aes.Padding = PaddingMode.PKCS7;//默认
            // KeySize 默认 256
            aes.KeySize = 256;
            aes.BlockSize = 128;

            //创建解密器对象
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream memoryStream = new MemoryStream(source))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    byte[] buffer_T = new byte[source.Length];/*--s1:创建临时数组，用于包含可用字节+无用字节--*/

                    int i = cryptoStream.Read(buffer_T, 0, source.Length);/*--s2:对加密数组进行解密，并通过i确定实际多少字节可用--*/

                    //cryptoStream.FlushFinalBlock();     //使用Read模式不能有此句，但write模式必须要有。

                    decrypted = new byte[i];/*--s3:创建只容纳可用字节的数组--*/

                    Array.Copy(buffer_T, 0, decrypted, 0, i);/*--s4:从bufferT拷贝出可用字节到decrypted--*/
                }
            }
        }
        return decrypted;
    }


    //***---声明CancellationTokenSource对象--***//
    //private CancellationTokenSource cts;//using System.Threading;引用
    //public Task EnOrDecryptFileAsync(Stream inStream, long inStream_Seek, Stream outStream, long outStream_Seek, string password, ActionDirection direction, IProgress<int> progress)
    //{
    //    /***---实例化CancellationTokenSource对象--***/
    //    cts?.Dispose();//cts为空，不动作，cts不为空，执行Dispose。
    //    cts = new CancellationTokenSource();

    //    Task mytask = new Task(
    //      () =>
    //      {
    //          EnOrDecryptFile(inStream, inStream_Seek, outStream, outStream_Seek, password, direction, progress);
    //      }, cts.Token, TaskCreationOptions.LongRunning);
    //    mytask.Start();
    //    return mytask;
    //}
    //private void EnOrDecryptFile(Stream inStream, long inStream_Seek, Stream outStream, long outStream_Seek, string password, ActionDirection direction, IProgress<int> progress)
    //{
    //    if (inStream == null || outStream == null)
    //        throw new ArgumentException("输入流与输出流是必须的");
    //    //--调整流的位置(通常是为了避开文件头部分)
    //    inStream.Seek(inStream_Seek, SeekOrigin.Begin);
    //    outStream.Seek(outStream_Seek, SeekOrigin.Begin);
    //    //用于记录处理进度
    //    long total_Length = inStream.Length - inStream_Seek;
    //    long totalread_Length = 0;
    //    //初始化报告进度
    //    progress.Report(0);

    //    var keyAndIv = GenerateKeyAndIV(password);

    //    using (Aes aes = Aes.Create())
    //    {
    //        //设定密钥和向量
    //        (aes.Key, aes.IV) = GenerateKeyAndIV(password);
    //        //创建加密器解密器对象（加解密方法不同处仅仅这一句话）
    //        ICryptoTransform cryptor;
    //        if (direction == ActionDirection.EnCrypt)
    //            cryptor = aes.CreateEncryptor(aes.Key, aes.IV);
    //        else
    //            cryptor = aes.CreateDecryptor(aes.Key, aes.IV);
    //        using (CryptoStream cstream = new CryptoStream(outStream, cryptor, CryptoStreamMode.Write))
    //        {
    //            byte[] buffer = new byte[512 * 1024];//每次读取512kb的数据
    //            int readLen = 0;
    //            while ((readLen = inStream.Read(buffer, 0, buffer.Length)) != 0)
    //            {
    //                // 向加密流写入数据
    //                cstream.Write(buffer, 0, readLen);
    //                totalread_Length += readLen;
    //                //汇报处理进度
    //                if (progress != null)
    //                {
    //                    long per = 100 * totalread_Length / total_Length;
    //                    progress.Report(Convert.ToInt32(per));
    //                }
    //            }
    //        }
    //    }
    //}
}
