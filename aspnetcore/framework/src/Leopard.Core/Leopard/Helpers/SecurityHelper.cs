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
        /// <summary>
        /// 是否是sql安全的参数
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static bool IsSqlSafeParam(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 是否是sql注入风险字符串
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static bool IsSqlDangerString(string str)
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
