using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Crypto
{
    /// <summary>
    /// base64 编码 解码
    /// （使用 CryptoGuide 静态类进行访问）
    /// </summary>
    public class Base64
    {
        /// <summary>
        /// 使用 CryptoGuide 静态类进行访问
        /// </summary>
        internal Base64() { }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">对plaintext进行编码采用的编码方式</param>
        /// <param name="plaintext">待编码的明文</param>
        /// <returns>编码后的字符串</returns>
        public string Encode(Encoding encodeType, string plaintext)
        {
            byte[] bytes = encodeType.GetBytes(plaintext);
            return Encode(bytes);
        }

        /// <summary>
        /// Base64 编码
        /// </summary>
        /// <param name="source">待编码的明文byte[]</param>
        /// <returns>编码后的字符串</returns>
        public string Encode(byte[] source)
        {
            string encode = string.Empty;
            try
            {
                encode = Convert.ToBase64String(source);
            }
            catch
            {
                throw;
            }
            return encode;
        }

        /// <summary>
        /// Base64 解码
        /// </summary>
        /// <param name="encodeType">对cipherText进行解码采用的解码方式，注意和编码时采用的方式一致</param>
        /// <param name="cipherText">待解密的密文</param>
        /// <returns>解码后的字符串</returns>
        public string Decode(Encoding encodeType, string cipherText)
        {
            string decode = string.Empty;
            byte[] bytes = Decode(cipherText);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                throw;
            }
            return decode;
        }

        /// <summary>
        /// Base64 解码
        /// </summary>
        /// <param name="cipherText">待解密的密文</param>
        /// <returns>解码后的字符串</returns>
        public byte[] Decode(string cipherText)
        {
            byte[] decode = null;
            try
            {
                decode = Convert.FromBase64String(cipherText);
            }
            catch
            {
                throw;
            }
            return decode;
        }
    }
}
