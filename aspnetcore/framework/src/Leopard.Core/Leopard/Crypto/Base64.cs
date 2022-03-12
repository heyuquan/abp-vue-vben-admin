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
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="originalText">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public string Base64Encode(string originalText)
        {
            return Base64Encode(Encoding.UTF8, originalText);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="originalText">待加密的明文</param>
        /// <returns></returns>
        public string Base64Encode(Encoding encodeType, string originalText)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(originalText);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                throw;
            }
            return encode;
        }

        ///// <summary>
        ///// Base64加密
        ///// </summary>
        ///// <param name="source">待加密的明文</param>
        ///// <returns></returns>
        //public string Base64Encode(byte[] source)
        //{
        //    string encode = string.Empty;
        //    try
        //    {
        //        encode = Convert.ToBase64String(source);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return encode;
        //}

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="cipherText">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public string Base64Decode(string cipherText)
        {
            return Base64Decode(Encoding.UTF8, cipherText);
        }


        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="cipherText">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public string Base64Decode(Encoding encodeType, string cipherText)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(cipherText);
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
    }
}
