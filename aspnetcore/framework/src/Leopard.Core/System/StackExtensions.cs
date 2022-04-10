using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StackExtensions
    {
        /// <summary>
        /// 返回堆栈对一个元素 （不进行移除）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryPeek<T>(this Stack<T> stack, out T value)
        {
            value = default;

            if (stack.Count > 0)
            {
                value = stack.Peek();
                return true;
            }

            return false;
        }

        /// <summary>
        /// 返回堆栈对一个元素，并且从堆栈移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryPop<T>(this Stack<T> stack, out T value)
        {
            value = default;

            if (stack.Count > 0)
            {
                value = stack.Pop();
                return true;
            }

            return false;
        }
    }
}
