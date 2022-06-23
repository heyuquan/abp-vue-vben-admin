using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Leopard.Validatetion
{
    // [.net core]C#MVC框架中的model验证，在其他架构中使用（Winform或控制台）
    // https://blog.csdn.net/weixin_33376883/article/details/113382540

    // [.net framework] 文中 EntityValidator 的封装
    // https://www.manongdao.com/article-828854.html

    /// <summary>
    /// 注解验证[DataAnnotations]帮助类
    /// </summary>
    public class ValidateHelper
    {
        /// <summary>
        /// 自动验证
        /// </summary>
        /// <param name="value">要验证的类型名称</param>
        /// <returns>错误结果</returns>
        public static ValidResult IsValid(object value)
        {
            ValidResult result = new ValidResult();
            try
            {
                var validationContext = new ValidationContext(value);
                var results = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(value, validationContext, results, true);

                if (!isValid)
                {
                    result.IsVaild = false;
                    result.ErrorMembers = new List<ErrorMember>();
                    foreach (var item in results)
                    {
                        result.ErrorMembers.Add(new ErrorMember()
                        {
                            ErrorMessage = GetDisplayName(value, item.MemberNames.FirstOrDefault()) + " " + item.ErrorMessage,
                            ErrorMemberName = item.MemberNames.FirstOrDefault()
                            //ErrorMemberName = GetDisplayName(value, item.MemberNames.FirstOrDefault())
                        });
                    }
                }
                else
                {
                    result.IsVaild = true;
                }
            }
            catch (Exception ex)
            {
                result.IsVaild = false;
                result.ErrorMembers = new List<ErrorMember>();
                result.ErrorMembers.Add(new ErrorMember()
                {
                    ErrorMessage = ex.Message,
                    ErrorMemberName = "Internal error"
                });
            }

            return result;
        }
        /// <summary>
        /// 获取类型的属性的DisplayName
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="propertyDisplayName"></param>
        /// <returns></returns>
        public static string GetDisplayName(object modelType, string propertyDisplayName)
        {
            return (System.ComponentModel.TypeDescriptor.GetProperties(modelType)[propertyDisplayName].Attributes[typeof(System.ComponentModel.DisplayNameAttribute)] as System.ComponentModel.DisplayNameAttribute).DisplayName;
        }
    }

    /// <summary>
    /// 错误名称和信息
    /// </summary>
    public class ErrorMember
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 错误属性名称
        /// </summary>
        public string ErrorMemberName { get; set; }
    }

    /// <summary>
    /// 验证结果
    /// </summary>
    public class ValidResult
    {
        /// <summary>
        /// 错误集合
        /// </summary>
        public List<ErrorMember> ErrorMembers { get; set; }
        /// <summary>
        /// 错误状态，true表示验证成功
        /// </summary>
        public bool IsVaild { get; set; }
    }
}
