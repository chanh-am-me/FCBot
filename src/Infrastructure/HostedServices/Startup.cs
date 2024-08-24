using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.HostedServices;

internal static class Startup
{
    internal static IServiceCollection AddBackgroundService(this IServiceCollection services)
        => services.AddHostedService<ReadMessagesHostedService>();
}
