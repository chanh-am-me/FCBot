using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Infrastructure.Extensions;

public static class ValidationContextExtension
{
    public static IEnumerable<ValidationResult> Required(this ValidationContext validationContext, params string[] ignoreProperties)
    {
        foreach (PropertyInfo propertyInfo in validationContext.ObjectType.GetProperties())
        {
            if (ignoreProperties.Contains(propertyInfo.Name, StringComparer.OrdinalIgnoreCase))
            {
                continue;
            }

            Type propertyType = propertyInfo.PropertyType;

            if (propertyType == typeof(bool))
            {
                continue;
            }

            object? propValue = propertyInfo.GetValue(validationContext.ObjectInstance);
            object? defaultVal;
            string message = $"{propertyInfo.Name} of {validationContext.ObjectType.FullName} is required";
            if (propertyType == typeof(string))
            {
                if (string.IsNullOrEmpty(propValue?.ToString()))
                {
                    yield return new ValidationResult(
                    message,
                    new[] { propertyInfo.Name });
                }
            }
            else
            {
                defaultVal = propertyType.IsNullableType() ? null : Activator.CreateInstance(propertyType);

                if (propValue?.Equals(defaultVal) == true)
                {
                    yield return new ValidationResult(
                        message,
                        new[] { propertyInfo.Name });
                }
            }
        }
    }
}
