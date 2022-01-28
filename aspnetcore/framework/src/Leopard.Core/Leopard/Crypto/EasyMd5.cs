using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Leopard.Crypto
{
    /// <summary>
    /// MD5 Hash和验证
    /// </summary>
    public class EasyMd5
    {
        private static string GetMd5Hash(byte[] data)
        {
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        private static bool VerifyMd5Hash(byte[] data, string hash)
        {
            return 0 == StringComparer.OrdinalIgnoreCase.Compare(GetMd5Hash(data), hash);
        }

        /// <summary>
        /// Md5 Hash（默认UTF8编码）
        /// </summary>
        /// <returns></returns>
        public static string Hash(string data)
        {
            return Hash(data, Encoding.UTF8);
        }
        public static string Hash(string data, Encoding encode)
        {
            using (var md5 = MD5.Create())
                return GetMd5Hash(md5.ComputeHash(encode.GetBytes(data)));
        }
        public static string Hash(FileStream data)
        {
            using (var md5 = MD5.Create())
                return GetMd5Hash(md5.ComputeHash(data));
        }

        /// <summary>
        /// Md5 验证（默认UTF8编码）
        /// </summary>
        public static bool Verify(string data, string hash)
        {
            return Verify(data, hash, Encoding.UTF8);
        }

        public static bool Verify(string data, string hash, Encoding encode)
        {
            using (var md5 = MD5.Create())
                return VerifyMd5Hash(md5.ComputeHash(encode.GetBytes(data)), hash);
        }

        public static bool Verify(FileStream data, string hash)
        {
            using (var md5 = MD5.Create())
                return VerifyMd5Hash(md5.ComputeHash(data), hash);
        }
    }
}
