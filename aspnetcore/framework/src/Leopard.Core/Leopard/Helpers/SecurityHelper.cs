using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Leopard.Helpers
{
    public static class SecurityHelper
    {
        public static bool IsSafeSqlParam(string value)
        {
            return !Regex.IsMatch(value, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 是否含有多余的字符 防止SQL注入
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static bool IsBadString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            //列举一些特殊字符串
            const string badChars = "@,*,#,$,!,+,',=,--,%,^,&,?,(,), <,>,[,],{,},/,\\,;,:,\",\"\",delete,update,drop,alert,select";
            var arraryBadChar = badChars.Split(',');
            return arraryBadChar.Any(t => !str.Contains(t));
        }
    }
}
