using Infrastructure.Channels.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Channels;

internal static class Startup
{
    internal static IServiceCollection AddModule(this IServiceCollection services)
    {
        services.AddScoped<IChannelService, ChannelService>();
        return services;
    }
}
