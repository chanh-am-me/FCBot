using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceScopeFactoryExtension
{
    public static TService GetService<TService>(this IServiceScopeFactory serviceScopeFactory)
         where TService : notnull
    {
        IServiceScope scope = serviceScopeFactory.CreateScope();
        return scope.ServiceProvider.GetRequiredService<TService>();
    }
}
