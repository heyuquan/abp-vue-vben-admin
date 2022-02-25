using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Leopard.Crypto
{
    /// <summary>
    /// MD5 Hash和验证
    /// （使用 CryptoGuide 静态类进行访问）
    /// </summary>
    public class EasyMd5
    {
        /// <summary>
        /// 使用 CryptoGuide 静态类进行访问
        /// </summary>
        internal EasyMd5() { }

        private string GetMd5Hash(byte[] data)
        {
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        private bool VerifyMd5Hash(byte[] data, string hash)
        {
            return 0 == StringComparer.OrdinalIgnoreCase.Compare(GetMd5Hash(data), hash);
        }

        /// <summary>
        /// Md5 Hash（默认UTF8编码）
        /// </summary>
        /// <returns></returns>
        public string Hash(string data)
        {
            return Hash(data, Encoding.UTF8);
        }
        public string Hash(string data, Encoding encode)
        {
            using (var md5 = MD5.Create())
                return GetMd5Hash(md5.ComputeHash(encode.GetBytes(data)));
        }
        public string Hash(FileStream data)
        {
            using (var md5 = MD5.Create())
                return GetMd5Hash(md5.ComputeHash(data));
        }

        /// <summary>
        /// Md5 验证（默认UTF8编码）
        /// </summary>
        public bool Verify(string data, string hash)
        {
            return Verify(data, hash, Encoding.UTF8);
        }

        public bool Verify(string data, string hash, Encoding encode)
        {
            using (var md5 = MD5.Create())
                return VerifyMd5Hash(md5.ComputeHash(encode.GetBytes(data)), hash);
        }

        public bool Verify(FileStream data, string hash)
        {
            using (var md5 = MD5.Create())
                return VerifyMd5Hash(md5.ComputeHash(data), hash);
        }
    }
}
