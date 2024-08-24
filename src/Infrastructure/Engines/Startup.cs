using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Engines;

internal static class Startup
{
    internal static IServiceCollection AddEngine(this IServiceCollection service)
    {
        service.AddScoped<IWBotEngine, WBotEngine>();
        service.AddScoped<ITeleBotEngine, TeleBotEngine>();
        return service;
    }
}
