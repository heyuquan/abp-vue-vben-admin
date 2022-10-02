using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Helpers
{
    public static class UrlHelper
    {

        /**
         * Encode a URL segment with special chars replaced.
         */
        public static String UrlEncode(String value, Encoding encoding)
        {
            if (value == null)
            {
                return "";
            }


            String encoded = System.Web.HttpUtility.UrlEncode(value, encoding);
            return encoded.Replace("+", "%20").Replace("*", "%2A")
                          .Replace("~", "%7E").Replace("/", "%2F");
        }

        /**
         * Decode a URL segment with special chars replaced.
         */
        public static String UrlDecode(String value, Encoding encoding)
        {
            if (value == null)
            {
                return "";
            }


            String encoded = System.Web.HttpUtility.UrlDecode(value, encoding);
            return encoded.Replace("%20", "+").Replace("%2A", "*")
                          .Replace("%7E", "~").Replace("%2F", "/");
        }

    }
}
