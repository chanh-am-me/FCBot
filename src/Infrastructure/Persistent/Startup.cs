using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistent;

internal static class Startup
{
    internal static IServiceCollection AddPersistent(this IServiceCollection services)
    {
        services
        .AddOptions<DatabaseSettings>()
        .BindConfiguration(nameof(DatabaseSettings))
        .ValidateOnStart();

        return services.AddDbContext();
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        return services.AddDbContextPool<ApplicationDbContext>((provider, builder) =>
        {
            DatabaseSettings settings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            builder.UseNpgsql(settings.ConnectionString, (builder) => builder.MigrationsAssembly("Migration.Postgres"));
        });
    }
}
