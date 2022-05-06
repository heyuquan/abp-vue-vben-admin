using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Crypto
{
    // 为什么程序上都是十六进制编码？
    // https://zhuanlan.zhihu.com/p/461151285
    // 每个十六进制中的数字代表4个比特，你可以非常直观的从十六进制中知道对应的二进制是啥，
    // 比如给定一个十六进制数，假设其最后一位是9，那么你立刻就能知道将该十六进制数字转为二进制后最后四位是1001：
    // #、十六进制数字9对应的二进制为1001。
    // #、十六进制数字19对应的二进制为1 1001。
    // #、十六进制数字119对应的二进制为1 0001 1001

    /// <summary>
    /// 十六进制编码
    /// （使用 CryptoGuide 静态类进行访问）
    /// </summary>
    public class HexEncoding
    {
        internal HexEncoding() { }

        //private static string HexStr = "0123456789abcdef";
        //private static char[] HexCharArr = HexStr.ToCharArray();

        //public static string ByteArrToHex(byte[] btArr)
        //{
        //    char[] strArr = new char[btArr.Length * 2];
        //    int i = 0;
        //    foreach (byte bt in btArr)
        //    {
        //        strArr[i++] = HexCharArr[bt >> 4 & 0xf];
        //        strArr[i++] = HexCharArr[bt & 0xf];
        //    }
        //    return new string(strArr);
        //}

        //public static byte[] HexToByteArr(string hexStr)
        //{
        //    char[] charArr = hexStr.ToCharArray();
        //    byte[] btArr = new byte[charArr.Length / 2];
        //    int index = 0;
        //    for (int i = 0; i < charArr.Length; i++)
        //    {
        //        int highBit = HexStr.IndexOf(charArr[i]);
        //        int lowBit = HexStr.IndexOf(charArr[++i]);
        //        btArr[index] = (byte)(highBit << 4 | lowBit);
        //        index++;
        //    }
        //    return btArr;
        //}

        /// <summary>
        /// byte数组转为十六进制
        /// </summary>
        /// <param name="btArr"></param>
        /// <param name="isLower">是否小写</param>
        /// <returns></returns>
        public string ByteArrToHex(byte[] btArr, bool isLower)
        {
            StringBuilder sb = new StringBuilder();
            string format = isLower ? "x2" : "X2";
            foreach (byte b in btArr)
            {
                sb.Append(b.ToString(format));        // "x2"==小写十六进制。 "X2"==大写
            }
            return sb.ToString();
        }

        /// <summary>
        /// 十六进制转为byte数组
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        public byte[] HexToByteArr(string hexStr)
        {
            byte[] inputArr = new byte[hexStr.Length / 2];
            for (int i = 0; i < hexStr.Length / 2; i++)
            {
                int v = Convert.ToInt32(hexStr.Substring(i * 2, 2), 16);
                inputArr[i] = (byte)v;
            }
            return inputArr;
        }
    }
}
