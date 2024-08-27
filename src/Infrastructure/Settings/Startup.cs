using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Settings;

internal static class Startup
{
    internal static IServiceCollection AddSettings(this IServiceCollection services)
    {
        services
        .AddOptions<TelegramSettings>()
        .BindConfiguration(nameof(TelegramSettings))
        .ValidateOnStart();

        return services;
    }
}
