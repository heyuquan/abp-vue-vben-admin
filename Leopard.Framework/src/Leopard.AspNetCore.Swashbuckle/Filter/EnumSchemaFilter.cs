using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Leopard.AspNetCore.Swashbuckle.Filter
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                model.Enum.Clear();

                List<string> names = Enum.GetNames(context.Type).ToList();

                Dictionary<string, string> displayNameDic = this.GetNamesDescription(context.Type, names);

                foreach (var item in displayNameDic)
                {
                    model.Enum.Add(new OpenApiString($"{item.Key} = {item.Value}"));
                }
            }
        }

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetNamesDescription(Type t, List<string> names)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            names.ForEach(name =>
            {
                var memberInfo = t.GetMember(name);
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes == null || attributes.Length != 1)
                {
                    //如果没有定义描述，就取当前枚举值的对应名称
                    dic.Add(Convert.ToInt64(Enum.Parse(t, name)).ToString(), name);
                }
                else
                {
                    dic.Add(Convert.ToInt64(Enum.Parse(t, name)).ToString(), (attributes.Single() as DescriptionAttribute).Description);
                }
            });

            return dic;
        }
    }
}
