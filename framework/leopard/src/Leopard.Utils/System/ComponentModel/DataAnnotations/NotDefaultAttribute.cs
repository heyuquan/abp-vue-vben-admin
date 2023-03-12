// Creating a not-empty GUID validation attribute and a not-default validation attribute
// https://andrewlock.net/creating-an-empty-guid-validation-attribute/
// 另外在使用Required时可以结合AllowEmptyStrings属性一起使用。eg：[Required(AllowEmptyStrings = false)]

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 对于不可空类型的Guid和DateTime的dto字段，如果不设置值（不传入），会默认填上Guid.Empty和DateTime.MinValue
    /// 使用此特性，要求此字段，必须传入一个有效值 。
    /// </summary>
    [AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
    public class NotDefaultAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must set value，can not use default value";
        public NotDefaultAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            //NotDefault doesn't necessarily mean required
            if (value is null)
            {
                return true;
            }

            var type = value.GetType();
            if (type.IsValueType)
            {
                var defaultValue = Activator.CreateInstance(type);
                return !value.Equals(defaultValue);
            }

            // non-null ref type
            return true;
        }
    }
}
