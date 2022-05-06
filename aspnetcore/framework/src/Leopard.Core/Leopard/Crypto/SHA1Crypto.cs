using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Crypto
{
    /// <summary>
    /// 安全哈希算法（Secure Hash Algorithm）主要适用于数字签名标准 
    /// </summary>
    public class SHA1Crypto
    {
        /// <summary>
        /// 使用 CryptoGuide 静态类进行访问
        /// </summary>
        internal SHA1Crypto() { }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string EncryptSHA1(string input)
        {
            byte[] inputBytes = Encoding.Default.GetBytes(input);

            SHA1 sha = new SHA1CryptoServiceProvider();

            byte[] result = sha.ComputeHash(inputBytes);

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < result.Length; i++)
            {
                sBuilder.Append(result[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        /// <summary>
        /// SHA1验证
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encrypt32Str"></param>
        /// <returns></returns>
        public bool VerifySha1Hash(string input, string encrypt32Str)
        {
            string hashOfInput = EncryptSHA1(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, encrypt32Str))
                return true;
            else
                return false;
        }
    }
}
