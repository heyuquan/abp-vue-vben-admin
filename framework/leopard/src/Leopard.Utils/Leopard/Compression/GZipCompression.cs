using System.IO;
using System.IO.Compression;

namespace Leopard.Compression
{

    // 1、GZIP：可以减少存储空间，通过网络传输文件时，可以减少传输的时间。
    // 2、ZIP：支持基于对称加密系统的一个简单的密码，已知有严重的缺陷，已知明文攻击，字典攻击和暴力攻击。

    // 压缩算法对比
    // Brotli VS Gzip (https://www.zhanzhangb.com/1645.html)
    // #、Brotli 要求浏览器必须支持与 HTTPS 一起使用，这也是他相比在浏览器支持量上比 Gzip 少的原因。毕竟 Gzip 同时支持 HTTP 和 HTTPS。
    // #、Brotli 的压缩率更高。根据 Certsimple 的研究，用 Brotli 压缩的 Javascript 文件比 Gzip 小 14％，HTML 文件比 Gzip 小 21％，CSS 文件比 Gzip 小 17％。
    // #、Brotli 压缩操作所花费的时间会随着压缩级别的增加而增加

    // 注意：图像不应该被 Gzip 或 Brotli 压缩，因为它们已经被压缩，再次压缩将使其尺寸变大。

    // accept-encoding：gzip,deflate,br
    // .net 5启用响应压缩 （https://blog.csdn.net/weixin_33277597/article/details/122919497）
    // https://docs.microsoft.com/zh-cn/aspnet/core/performance/response-compression?view=aspnetcore-6.0&viewFallbackFrom=aspnetcore-2.1


    // DeflateStream 和 BrotliStream 相关代码
    // https://mp.weixin.qq.com/s/vOapo0BaXdWHYO4rLWaMpA


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
