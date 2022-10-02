using System;
using System.Text;

namespace Leopard
{
    public sealed class Constants
    {
        public static Encoding DEFAULT_ENCODING = Encoding.UTF8;

        public const string Framework_Name = "Leopard";

        public const string String_Empty = "";

        public const string CHARSET_UTF8 = "utf-8";

        public const string ACCEPT_ENCODING = "Accept-Encoding";
        public const string CONTENT_ENCODING = "Content-Encoding";
        public const string CONTENT_ENCODING_GZIP = "gzip";

        public const string CTYPE_DEFAULT = "application/octet-stream";
        public const string CTYPE_FORM_DATA = "application/x-www-form-urlencoded";
        public const string CTYPE_FILE_UPLOAD = "multipart/form-data";
        public const string CTYPE_TEXT_XML = "text/xml";
        public const string CTYPE_TEXT_PLAIN = "text/plain";
        public const string CTYPE_APPLICATION_XML = "application/xml";
        public const string CTYPE_APP_JSON = "application/json";

        public const string TIMESTAMP = "timestamp";
        public const string VERSION = "v";
        public const string SIGN = "sign";

        public const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string DATE_TIME_MS_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

        public const int READ_BUFFER_SIZE = 1024 * 4;

        // sql server datetime类型的的范围不到0001-01-01，所以转成1970-01-01
        // 考虑：在保存数据的时候，统一给没有值的dateTime 赋默认值 MinTime
        /// <summary>
        /// 最小时间
        /// </summary>
        public static DateTime MinTime = DateTime.Parse("1970-01-01 00:00:00");

        /// <summary>
        /// 媒体协议类型
        /// </summary>
        public class MediaType
        {
            public const string Xml = "application/xml";

            public const string Json = "application/json";

            public const string Bson = "application/bson";

            public const string MessagePack = "application/x-msgpack";

            public const string ProtoBuf = "application/x-protobuf";

            public const string FormUrl = "application/x-www-form-urlencoded";

            public const string OctetStream = "application/octet-stream";
        }

        /// <summary>
        /// 请求限制
        /// </summary>
        public class RequestLimit
        {
            /// <summary>
            /// 请求中单个字段值，最大长度（单位 byte），默认 20M
            /// </summary>
            public const int MaxValueLength_Byte = 20 * 1024 * 1024;  

            /// <summary>
            /// 请求的整个正文，最大长度（单位 byte），默认 200M
            /// </summary>
            public const int MaxBodyLength_Byte = 200 * 1024 * 1024;  
        }
    }
}
