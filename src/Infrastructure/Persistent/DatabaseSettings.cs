using Infrastructure.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistent;

public class DatabaseSettings : IValidatableObject
{
    public string ConnectionString { get; set; } = default!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => validationContext.Required();
}
