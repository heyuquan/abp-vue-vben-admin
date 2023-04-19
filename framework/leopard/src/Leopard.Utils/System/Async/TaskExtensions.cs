using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Async
{
    public static partial class TaskExtensions
    {
        /// <summary>
        /// Awaits a task synchronously. 
        /// Shortcut for <code>task.GetAwaiter().GetResult()</code>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Await(this Task task)
        {
            task.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Awaits a task synchronously and returns the result. 
        /// Shortcut for <code>task.GetAwaiter().GetResult()</code>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Await<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Awaits a task synchronously. 
        /// Shortcut for <code>task.GetAwaiter().GetResult()</code>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Await(this ValueTask task)
        {
            task.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Awaits a task synchronously and returns the result. 
        /// Shortcut for <code>task.GetAwaiter().GetResult()</code>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Await<T>(this ValueTask<T> task)
        {
            return task.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Task的超时
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="milliseconds">超时时间（毫秒）</param>
        /// <returns>未超时，返回原Task对象；超时，则返回T的默认值</returns>
        public static async Task<T> Timeout<T>(this Task<T> task, int milliseconds)
        {
            // 参考：https://mp.weixin.qq.com/s/xfBUwFcvcwdc49_VZUfRVw

            var timeoutTask = Task.Delay(milliseconds);
            var allTasks = new List<Task> { task, timeoutTask };
            Task finishedTask = await Task.WhenAny(allTasks);
            if (finishedTask == timeoutTask)
            {
                return default(T);
            }

            return await task;
        }
    }
}
