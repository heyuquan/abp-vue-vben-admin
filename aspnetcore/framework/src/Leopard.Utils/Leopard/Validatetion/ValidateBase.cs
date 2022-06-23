using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leopard.Validatetion
{
    public class ValidateBase
    {
        public virtual void ValidateThrowError()
        {
            var validResult = ValidateHelper.IsValid(this);
            if (!validResult.IsVaild)
            {
                StringBuilder sbrErrors = new StringBuilder();
                foreach (ErrorMember errorMember in validResult.ErrorMembers)
                {
                    sbrErrors.AppendLine(errorMember.ErrorMessage);
                }
                throw new ValidationException(sbrErrors.ToString());
            }
        }

        public virtual ValidResult Validate()
        {
            return ValidateHelper.IsValid(this);
        }
    }
}
