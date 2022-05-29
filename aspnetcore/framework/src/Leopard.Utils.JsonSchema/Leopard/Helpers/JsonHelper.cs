using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;

namespace Leopard.Helpers
{
    public static partial class JsonHelper
    {
        /// <summary>
        /// 从类生成json schema
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string JSchemaGen<T>()
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(typeof(T));
            return null;
        }

        /// <summary>
        /// 验证对象是否符合 JSchema 定义
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool JSchemaVaidate<T>(T obj, JSchema schema)
        {
            JObject jObject = JObject.FromObject(obj);
            return jObject.IsValid(schema);
        }

        /// <summary>
        /// 验证 json string 是否符合 JSchema 定义
        /// </summary>
        /// <param name="json"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool JSchemaVaidate(string json, JSchema schema)
        {
            JObject jObject = JObject.Parse(json);
            return jObject.IsValid(schema);
        }
    }
}
