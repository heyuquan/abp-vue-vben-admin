using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard
{
    public sealed class Constants
    {
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
        public const string CTYPE_APP_JSON = "application/json";

        public const string TIMESTAMP = "timestamp";
        public const string VERSION = "v";
        public const string SIGN = "sign";

        public const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string DATE_TIME_MS_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

        public const int READ_BUFFER_SIZE = 1024 * 4;

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
    }
}
