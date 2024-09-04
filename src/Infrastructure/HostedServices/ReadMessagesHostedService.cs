using Infrastructure.Engines;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.HostedServices;

public class ReadMessagesHostedService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //ReceiveMessages();
        //return;
        IWBotEngine engine = serviceScopeFactory.GetService<IWBotEngine>();
        //ChannelConfig channel = ChannelConfig.Default;
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                //await engine.ReadLastedMessagesAsync(channel);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Exception: " + ex.Message);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
