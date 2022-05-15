using Leopard.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Leopard.Crypto
{
    // MD5 32位，md5.ComputeHash(input) 转为hex十六进制，就是32位

    // MD5 16位，md5.ComputeHash(input) 转为hex十六进制，再取中间十六位.  即：Encrypt32(***).Substring(8, 16);

    // MD5 64位，md5.ComputeHash(input) 后取 Convert.ToBase64String

    /// <summary>
    /// MD5 Hash和验证（md5不可逆加密方式，所以没有解码方法）
    /// （使用 CryptoGuide 静态类进行访问）
    /// </summary>
    public class Md5Crypto : Singleton<Md5Crypto>
    {
        static Md5Crypto()
        {
            Instance = new Md5Crypto();
        }

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private byte[] Encrypt(byte[] input)
        {
            MD5 md5 = MD5.Create();
            return md5.ComputeHash(input);
        }

        /// <summary>
        /// 16位MD5加密 (默认utf-8)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Encrypt16(string input)
        {
            return Encrypt16(input, Constants.DEFAULT_ENCODING);
        }

        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public string Encrypt16(string input, Encoding encode)
        {
            return Encrypt16(encode.GetBytes(input));
        }

        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Encrypt16(byte[] input)
        {
            string md5_32 = Encrypt32(input);

            return md5_32.Substring(8, 16);
        }

        /// <summary>
        /// 32位MD5加密 (默认utf-8)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Encrypt32(string input)
        {
            return Encrypt32(input, Constants.DEFAULT_ENCODING);
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public string Encrypt32(string input, Encoding encode)
        {
            return Encrypt32(encode.GetBytes(input));
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Encrypt32(byte[] input)
        {
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = Encrypt(input);

            return CryptoGuide.Hex.ByteArrToHex(s, true);
        }

        /// <summary>
        /// 64位MD5加密 (默认utf-8)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Encrypt64(string input)
        {
            return Encrypt64(input, Constants.DEFAULT_ENCODING);
        }

        /// <summary>
        /// 64位MD5加密 (默认utf-8)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public string Encrypt64(string input, Encoding encode)
        {
            return Encrypt64(encode.GetBytes(input));
        }

        /// <summary>
        /// 64位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Encrypt64(byte[] input)
        {
            byte[] result = Encrypt(input);
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// 验证Md5 hash
        /// </summary>
        /// <param name="input">原字符串</param>
        /// <param name="encrypt16Str">原字符串的md5码</param>
        /// <returns></returns>
        public bool Verify16Hash(string input, string encrypt16Str)
        {
            return Verify16Hash(input, encrypt16Str, Constants.DEFAULT_ENCODING);
        }

        /// <summary>
        /// 验证Md5 hash
        /// </summary>
        /// <param name="input">原字符串</param>
        /// <param name="encrypt16Str">原字符串的md5码</param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public bool Verify16Hash(string input, string encrypt16Str, Encoding encode)
        {
            string hashOfInput = Encrypt16(input, encode);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, encrypt16Str))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 验证Md5 hash
        /// </summary>
        /// <param name="input">原字符串</param>
        /// <param name="encrypt32Str">原字符串的md5码</param>
        /// <returns></returns>
        public bool Verify32Hash(string input, string encrypt32Str)
        {
            return Verify32Hash(input, encrypt32Str, Constants.DEFAULT_ENCODING);
        }

        /// <summary>
        /// 验证Md5 hash
        /// </summary>
        /// <param name="input">原字符串</param>
        /// <param name="encrypt32Str">原字符串的md5码</param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public bool Verify32Hash(string input, string encrypt32Str, Encoding encode)
        {
            string hashOfInput = Encrypt32(input, encode);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, encrypt32Str))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 验证Md5 hash
        /// </summary>
        /// <param name="input">原字符串</param>
        /// <param name="encrypt64Str">原字符串的md5码</param>
        /// <returns></returns>
        public bool Verify64Hash(string input, string encrypt64Str)
        {
            return Verify64Hash(input, encrypt64Str, Constants.DEFAULT_ENCODING);
        }

        /// <summary>
        /// 验证Md5 hash
        /// </summary>
        /// <param name="input">原字符串</param>
        /// <param name="encrypt64Str">原字符串的md5码</param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public bool Verify64Hash(string input, string encrypt64Str, Encoding encode)
        {
            string hashOfInput = Encrypt64(input, encode);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, encrypt64Str))
                return true;
            else
                return false;
        }
    }
}
