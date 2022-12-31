using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflect
{
    /// <summary>
    /// 动态编译类
    /// </summary>
    public class Dynamic
    {
        /// <summary>
        /// 动态赋值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetValue(object obj, string fieldName, object value)
        {
            FieldInfo info = obj.GetType().GetField(fieldName);
            info.SetValue(obj, value);
        }
        /// <summary>
        /// 泛型动态赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetValue<T>(object obj, string fieldName, T value)
        {
            FieldInfo info = obj.GetType().GetField(fieldName);
            info.SetValue(obj, value);
        }
        /// <summary>
        /// 动态取值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetValue(object obj, string fieldName)
        {
            FieldInfo info = obj.GetType().GetField(fieldName);
            return info.GetValue(obj);
        }
        /// <summary>
        /// 动态取值泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static T GetValue<T>(object obj, string fieldName)
        {
            FieldInfo info = obj.GetType().GetField(fieldName);
            return (T)info.GetValue(obj);
        }
    }
}
