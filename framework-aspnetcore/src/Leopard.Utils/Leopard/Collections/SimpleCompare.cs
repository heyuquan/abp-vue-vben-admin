using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Collections
{
    // C# Distinct使用，支持对象的相等比较
    // https://www.cnblogs.com/danlis/p/5353749.html
    // 使用：**.Distinct(new SimpleCompare<DrawPlay>((x, y) => (x != null && y != null && x.UserID == y.UserID))).ToList()
    // 大部分场景下可以通用

    // C# List对象用Distinct方法为 指定某字段去重复
    // https://blog.csdn.net/lybwwp/article/details/102663074
    // 做特殊定义，特别是 需要 GetHashCode 方法的地方


    public delegate bool CompareDelegate<T>(T x, T y);
    public class SimpleCompare<T> : IEqualityComparer<T>
    {
        private CompareDelegate<T> _compare;
        public SimpleCompare(CompareDelegate<T> d)
        {
            this._compare = d;
        }

        public bool Equals(T x, T y)
        {
            if (_compare != null)
            {
                return this._compare(x, y);
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
