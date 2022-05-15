using ObjectsComparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Helpers
{
    public class Result
    {
        public bool IsEqual { get; set; }
        public string Msg { get; set; }
    }

    /// <summary>
    /// 比较器帮助类
    /// </summary>
    public static class ComparerHelper
    {
        public static Result ObjectCompare<T>(T t, T s)
        {
            Result result = new Result();
            var comparer = new ObjectsComparer.Comparer<T>();
            IEnumerable<Difference> differences;
            bool isEqual = comparer.Compare(t, s, out differences);
            result.IsEqual = isEqual;
            if (!isEqual)
            {
                string differencesMsg = string.Join(Environment.NewLine, differences);
                result.Msg = differencesMsg;
            }
            return result;
        }
    }
}
