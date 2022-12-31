using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Helpers
{
    public static class Utf8BomHelper
    {
        /// <summary>
        /// 判断 utf-8 文档是否含bom
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool HasBom(byte[] bytes)
        {
            if (bytes.Length < 3)
            {
                return false;
            }

            if (!(bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 从流中读取不含bom的数据 （跳过前面的bom标识字符）
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ReadStringFromStreamWithoutBom(Stream stream)
        {
            var bytes = stream.ReadToEnd();
            var skipCount = HasBom(bytes) ? 3 : 0;
            return Encoding.UTF8.GetString(bytes, skipCount, bytes.Length - skipCount);
        }

        /// <summary>
        /// 从byte数组中读取不含bom的数据 （跳过前面的bom标识字符）
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ReadStringFromByteWithoutBom(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            var skipCount = HasBom(bytes) ? 3 : 0;
            return Encoding.UTF8.GetString(bytes, skipCount, bytes.Length - skipCount);
        }
    }
}
