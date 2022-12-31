using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using System.Linq;

namespace Leopard.Helpers
{
    public static class JSchemaHelper
    {
        /// <summary>
        /// 从 Type 生成 json schema string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GenSchemaFromType<T>()
        {
            var schema = JsonSchema.FromType<T>();
            return schema.ToJson();
        }

        /// <summary>
        /// 从 json schema string 生成 cs类
        /// </summary>
        /// <returns></returns>
        public static string GenCodeFromSchema(string jsonSchemaStr)
        {
            var schema = JsonSchema.FromJsonAsync(jsonSchemaStr).ConfigureAwait(false).GetAwaiter().GetResult();
            return GenCodeFromSchema(schema);
        }

        /// <summary>
        /// 从 json 生成 cs类
        /// </summary>
        /// <returns></returns>
        public static string GenCodeFromJson(string json)
        {
            var schema = JsonSchema.FromSampleJson(json);
            return GenCodeFromSchema(schema);
        }

        /// <summary>
        /// 从 JsonSchema 生成 cs类
        /// </summary>
        /// <returns></returns>
        public static string GenCodeFromSchema(JsonSchema schema)
        {
            var generator = new CSharpGenerator(schema, new CSharpGeneratorSettings
            {
                ClassStyle = CSharpClassStyle.Poco
            });
            var code = generator.GenerateFile();
            return code;
        }

        /// <summary>
        /// 验证对象是否符合 JSchema 定义
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool Vaidate<T>(T obj, JsonSchema schema)
        {
            return Vaidate(JsonHelper.Serialize(obj),schema);
        }

        /// <summary>
        /// 验证 json string 是否符合 JSchema 定义
        /// </summary>
        /// <param name="json"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool Vaidate(string json, JsonSchema schema)
        {
            var errors = schema.Validate(json);
            return !errors.Any();
        }
    }
}
