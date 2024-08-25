using Infrastructure.Engines;
using Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.HostedServices;

public class ReadMessagesHostedService : BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    public ReadMessagesHostedService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IServiceScope scope = serviceProvider.CreateScope();
        IWBotEngine engine = scope.ServiceProvider.GetRequiredService<IWBotEngine>();
        ChannelConfig channel = ChannelConfig.Default;
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await engine.ReadLastedMessagesAsync(channel);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Exception: " + ex.Message);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
