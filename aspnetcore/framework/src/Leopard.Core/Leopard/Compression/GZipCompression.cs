using System.IO;
using System.IO.Compression;

namespace Leopard.Compression
{

    // 1、GZIP：可以减少存储空间，通过网络传输文件时，可以减少传输的时间。
    // 2、ZIP：支持基于对称加密系统的一个简单的密码，已知有严重的缺陷，已知明文攻击，字典攻击和暴力攻击。

    /// <summary>
    /// zip压缩
    /// </summary>
    public class GZipCompression
    {
        //压缩字节
        //1.创建压缩的数据流 
        //2.设定compressStream为存放被压缩的文件流,并设定为压缩模式
        //3.将需要压缩的字节写到被压缩的文件流
        /// <summary>
        /// 压缩字节
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] CompressBytes(byte[] bytes)
        {
            using (MemoryStream compressStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(compressStream, CompressionMode.Compress))
                    zipStream.Write(bytes, 0, bytes.Length);
                return compressStream.ToArray();
            }
        }

        //解压缩字节
        //1.创建被压缩的数据流
        //2.创建zipStream对象，并传入解压的文件流
        //3.创建目标流
        //4.zipStream拷贝到目标流
        //5.返回目标流输出字节
        /// <summary>
        /// 解压缩字节
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] bytes)
        {
            using (var compressStream = new MemoryStream(bytes))
            {
                using (var zipStream = new GZipStream(compressStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        return resultStream.ToArray();
                    }
                }
            }
        }
    }
}
