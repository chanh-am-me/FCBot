using Infrastructure.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Settings;

public class TelegramSettings : IValidatableObject
{
    public int AppId { get; set; }

    public string AppHash { get; set; } = default!;

    public string BotToken { get; set; } = default!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => validationContext.Required();
}
