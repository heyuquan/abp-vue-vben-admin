using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Leopard.AspNetCore.Swashbuckle.Filter
{
    /// <summary>
    /// 把枚举的 Description 特性的文字显示在swagger上
    /// </summary>
    public class EnumDescriptionFilter : IDocumentFilter
    {

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // add enum descriptions to result models
            foreach (var property in swaggerDoc.Components.Schemas.Where(x => x.Value?.Enum?.Count > 0))
            {
                IList<IOpenApiAny> propertyEnums = property.Value.Enum;
                if (propertyEnums != null && propertyEnums.Count > 0)
                {
                    property.Value.Description += $"==>{DescribeEnum(propertyEnums, property.Key)}";
                }
            }

            // add enum descriptions to input parameters
            foreach (var pathItem in swaggerDoc.Paths)
            {
                DescribeEnumParameters(pathItem.Value.Operations, swaggerDoc, context.ApiDescriptions, pathItem.Key);
            }
        }

        private void DescribeEnumParameters(IDictionary<OperationType, OpenApiOperation> operations, OpenApiDocument swaggerDoc, IEnumerable<ApiDescription> apiDescriptions, string path)
        {
            path = path.Trim('/');
            if (operations != null)
            {
                var pathDescriptions = apiDescriptions.Where(a => a.RelativePath == path);
                foreach (var oper in operations)
                {
                    var operationDescription = pathDescriptions.FirstOrDefault(a => a.HttpMethod.Equals(oper.Key.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    foreach (var param in oper.Value.Parameters)
                    {
                        var parameterDescription = operationDescription.ParameterDescriptions.FirstOrDefault(a => a.Name == param.Name);
                        if (parameterDescription != null && TryGetEnumType(parameterDescription.Type, out Type enumType))
                        {
                            var paramEnum = swaggerDoc.Components.Schemas.FirstOrDefault(x => x.Key == enumType.Name);
                            if (paramEnum.Value != null)
                            {
                                param.Description += DescribeEnum(paramEnum.Value.Enum, paramEnum.Key);
                            }
                        }
                    }
                }
            }
        }

        bool TryGetEnumType(Type type, out Type enumType)
        {
            if (type.IsEnum)
            {
                enumType = type;
                return true;
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType != null && underlyingType.IsEnum == true)
                {
                    enumType = underlyingType;
                    return true;
                }
            }
            else
            {
                Type underlyingType = GetTypeIEnumerableType(type);
                if (underlyingType != null && underlyingType.IsEnum)
                {
                    enumType = underlyingType;
                    return true;
                }
                else
                {
                    var interfaces = type.GetInterfaces();
                    foreach (var interfaceType in interfaces)
                    {
                        underlyingType = GetTypeIEnumerableType(interfaceType);
                        if (underlyingType != null && underlyingType.IsEnum)
                        {
                            enumType = underlyingType;
                            return true;
                        }
                    }
                }
            }

            enumType = null;
            return false;
        }

        Type GetTypeIEnumerableType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var underlyingType = type.GetGenericArguments()[0];
                if (underlyingType.IsEnum)
                {
                    return underlyingType;
                }
            }

            return null;
        }

        private Type GetEnumTypeByName(string enumTypeName)
        {
            var types = AppDomain.CurrentDomain
                .GetAssemblies().SelectMany(x => x.GetTypes());
            return types
                .FirstOrDefault(x => x.FullName == enumTypeName);
        }

        private string DescribeEnum(IList<IOpenApiAny> enums, string proprtyTypeName)
        {
            List<string> enumDescriptions = new List<string>();
            var enumType = GetEnumTypeByName(proprtyTypeName);
            if (enumType == null)
                return null;
            var allfields = enumType.GetFields();
            foreach (OpenApiInteger enumOption in enums)
            {
                int enumInt = enumOption.Value;
                string filedName = Enum.GetName(enumType, enumInt);
                string str = null;
                var field = allfields.FirstOrDefault(x => x.Name == filedName);
                var memberAttr = field?.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false).FirstOrDefault();
                if (memberAttr != null)
                {
                    var attr = (System.ComponentModel.DescriptionAttribute)memberAttr;
                    str = string.Format("{0} = {1}（{2}）", enumInt, filedName, attr.Description);
                }
                else
                {
                    str = string.Format("{0} = {1}", enumInt, filedName);
                }
                enumDescriptions.Add(str);
            }
            return string.Join(", ", enumDescriptions.ToArray());
        }
    }
}