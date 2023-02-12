using System.ComponentModel.DataAnnotations;

// Creating a not-empty GUID validation attribute and a not-default validation attribute
// https://andrewlock.net/creating-an-empty-guid-validation-attribute/
// 另外在使用Required时可以结合AllowEmptyStrings属性一起使用。eg：[Required(AllowEmptyStrings = false)]

namespace System.ComponentModel.Annotations
{
    [AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
    public class GuidNotEmptyAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must not be empty";
        public GuidNotEmptyAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            //NotEmpty doesn't necessarily mean required
            if (value is null)
            {
                return true;
            }

            switch (value)
            {
                case Guid guid:
                    return guid != Guid.Empty;
                default:
                    return true;
            }
        }
    }
}
