using Infrastructure.Engines;
using Infrastructure.HostedServices;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
        .AddEngine()
        .AddBackgroundService();
}
