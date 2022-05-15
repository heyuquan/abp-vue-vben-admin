using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;

namespace Leopard.Helpers
{
    #region JsonHelper
    public static class JsonHelper
    {

        // [循环引用问题]c＃-Json.Net中的PreserveReferencesHandling和ReferenceLoopHandling有什么区别？
        // https://www.itranslater.com/qa/details/2582250669625312256

        // 空值处理
        // NullValueHandling = NullValueHandling.Ignore,
        // https://blog.csdn.net/lovegonghui/article/details/50259439
        // eg："Name": null  整个字段会被删掉

        static JsonHelper()
        {
            JsonSerializerSettings common = new JsonSerializerSettings
            {
                // 日期类型默认格式化处理
                DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",

                // 设置序列化的最大层数
                MaxDepth = 10,
                // 避免循环引用 【指定如何处理循环引用，None--不序列化，Error-抛出异常，Serialize--仍要序列化】
                // Serialize--仍要序列化，配合 MaxDepth 来控制循环引用
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                // 如果您正在寻找更紧凑的JSON，并且将使用Json.Net或Web  API（或另一个兼容的库）对数据进行反序列化，
                // 则可以选择使用PreserveReferencesHandling.Objects。如果您的数据是没有重复引用的有向无环图，则无需任何设置。
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,

                // 于$id、$ref等是默认Json.NET的特殊属性，在反序列化时不会将其对应的值填充.所以将其设置为忽略
                // https://blog.csdn.net/weixin_30415113/article/details/98046246
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            };

            defaultSettings = common;

            JsonSerializerSettings temp = common.DeepClone();
            // 是否缩进显示
            temp.Formatting = Newtonsoft.Json.Formatting.Indented;
            defaultSettings_Indented = temp;
        }

        readonly static JsonSerializerSettings defaultSettings = null;

        // todo 改为 深拷贝
        readonly static JsonSerializerSettings defaultSettings_Indented = null;

        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isIndented">是否缩进显示</param>
        /// <returns></returns>
        public static string Serialize(object obj, bool isIndented = false)
        {
            JsonSerializerSettings settings = isIndented ? defaultSettings_Indented : defaultSettings;

            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this string json) where T : class
        {
            json = json.Replace("&nbsp;", "");
            return JsonConvert.DeserializeObject<T>(json, defaultSettings);
        }

        public static JObject ToJObject(this string json)
        {
            return json == null ? JObject.Parse("{}") : JObject.Parse(json.Replace("&nbsp;", ""));
        }

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
    #endregion

    #region JsonConverter
    /// <summary>
    /// Json数据返回到前端js的时候，把数值很大的long类型转成字符串
    /// </summary>
    public class StringJsonConverter : JsonConverter
    {
        public StringJsonConverter() { }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value.CastTo<long>();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            string sValue = value.ToString();
            writer.WriteValue(sValue);
        }
    }

    /// <summary>
    /// DateTime类型序列化的时候，转成指定的格式
    /// </summary>
    public class DateTimeJsonConverter : JsonConverter
    {
        public DateTimeJsonConverter() { }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value.ToString().CastTo<DateTime>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            DateTime? dt = value as DateTime?;
            if (dt == null)
            {
                writer.WriteNull();
                return;
            }
            writer.WriteValue(dt.Value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
    #endregion
}
