using Kaneko.Core.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Kaneko.Dapper.Expressions
{
    public static class ModelValidationExtensions
    {
        public static bool IsValid(this object obj, out Exception exception)
        {
            exception = null;

            if (obj == null)
            {
                exception = new ArgumentNullException();
                return false;
            }

            var context = new ValidationContext(obj);
            var result = Validate(context);
            if (result == null)
                return true;

            exception = new Exception("【" + result.MemberNames.FirstOrDefault() + "】" + result.ErrorMessage);
            return false;
        }

        private static ValidationResult Validate(ValidationContext context)
        {
            var properties = context.ObjectType.GetProperties();

            if (context.ObjectInstance is IValidatableObject valid)
            {
                var validationResults = valid.Validate(context).FirstOrDefault();
                if (validationResults != null)
                {
                    return validationResults;
                }
            }

            foreach (var property in properties)
            {
                var validationAttributes = property.GetKanekoAttributes<ValidationAttribute>();
                foreach (var attribute in validationAttributes)
                {
                    bool isValid = attribute.IsValid(property.GetValue(context.ObjectInstance));
                    if (!isValid)
                    {
                        return new ValidationResult(attribute.ErrorMessage, new[] { property.Name });
                    }
                }
            }

            return null;
        }
    }
}
