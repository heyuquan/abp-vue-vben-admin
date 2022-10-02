using Leopard.Helpers.IO;
using Leopard.Utils;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Leopard.Crypto
{
    public class CerMaker : Singleton<CerMaker>
    {
        static CerMaker()
        {
            Instance = new CerMaker();
        }


        #region 生成自签名的服务器证书
        // 【ASP.NET Core】自己编程来生成自签名的服务器证书
        // https://www.cnblogs.com/tcjiaan/p/16170870.html

        //// 先创建自签名证书
        //const string CER_FILE = "host.pfx";
        //const string PASSWD = "dagongji";
        //const string SUB_NAME = "CN=万年坑玩具厂.com.cn";
        //DateTime today = DateTime.Now;
        //DateTime endday = today.AddDays(365);
        //if(!File.Exists(CER_FILE))
        //{
        //    await CerMaker.CreateSslCertFileAsync(SUB_NAME, today, endday, CER_FILE, PASSWD);
        // }

        /// <summary>
        /// 创建证书文件（.pfx） 
        /// </summary>
        /// <param name="subName">证书名</param>
        /// <param name="bgDate">开始有效期</param>
        /// <param name="endDate">结束有效期</param>
        /// <param name="outFile">证书输出存放的完整路径</param>
        /// <param name="passWd">证书密码</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task CreateSslCertFileAsync(string subName,
                                       DateTime bgDate,
                                       DateTime endDate,
                                       string outFile,
                                       string passWd)
        {
            // 参数检查
            if (subName is null or { Length: < 3 })
            {
                throw new ArgumentNullException(nameof(subName));
            }
            if (endDate <= bgDate)
            {
                throw new ArgumentException("结束日期应大于开始日期");
            }
            if (FileHelper.IsFile(outFile))
            {
                throw new ArgumentException("应该输入文件完整路径", nameof(outFile));
            }

            // 随机密钥
            RSA key = RSA.Create(1024);
            // 创建CRT
            CertificateRequest crt = new(subName, key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            // 创建自签名证书
            var cert = crt.CreateSelfSigned(bgDate, endDate);
            // 将证书写入文件
            byte[] data = cert.Export(X509ContentType.Pfx, passWd);
            await File.WriteAllBytesAsync(outFile, data);
        }

        #endregion
    }
}
