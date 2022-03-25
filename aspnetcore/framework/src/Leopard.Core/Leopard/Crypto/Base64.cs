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

        #region  判断是否base64值

        private static readonly char[] CA = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".ToCharArray();
        private static readonly int[] IA = InitIA();
        private static int[] InitIA()
        {
            int len = 256;
            int[] a = new int[len];
            for (int i = 0; i < len; i++)
            {
                a[i] = -1;
            }

            for (int i = 0, iS = CA.Length; i < iS; i++)
            {
                a[CA[i]] = i;
            }
            a['='] = 0;
            return a;
        }

        /// <summary>
        /// 判断是否base64值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsBase64Value(string str)
        {
            // Check special case
            int sLen = str != null ? str.Length : 0;
            if (sLen == 0)
                return false;

            // Count illegal characters (including '\r', '\n') to know what size the returned array will be,
            // so we don't have to reallocate & copy it later.
            int sepCnt = 0; // Number of separator characters. (Actually illegal characters, but that's a bonus...)
            for (int i = 0; i < sLen; i++)  // If input is "pure" (I.e. no line separators or illegal chars) base64 this loop can be commented out.
            {
                char currentChar = str[i];
                if (currentChar >= IA.Length)
                {
                    return false;
                }

                if (IA[currentChar] < 0)
                {
                    sepCnt++;
                }

            }


            // Check so that legal chars (including '=') are evenly divideable by 4 as specified in RFC 2045.
            if ((sLen - sepCnt) % 4 != 0)
            {
                return false;
            }
            return true;
        }

        #endregion

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
