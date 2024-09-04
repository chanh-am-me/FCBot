using Infrastructure.Channels;
using Infrastructure.Engines;
using Infrastructure.Persistents;
using Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
        .AddPersistent()
        .AddSettings()
        .AddEngine()
        .AddModule();
    //.AddBackgroundService();
}
