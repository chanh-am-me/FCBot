using Infrastructure.Engines;
using Infrastructure.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistent;
using Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Npgsql;
using WTelegram;

namespace Infrastructure.HostedServices;

public class ReadMessagesHostedService(IServiceScopeFactory serviceScopeFactory, IOptions<TelegramSettings> telegramOptions, IOptions<DatabaseSettings> databaseOptions) : BackgroundService
{
    private readonly TelegramSettings telegramSettings = telegramOptions.Value;
    private readonly DatabaseSettings databaseSettings = databaseOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //ReceiveMessages();
        //return;
        IWBotEngine engine = serviceScopeFactory.GetService<IWBotEngine>();
        ChannelConfig channel = ChannelConfig.Default;
        using NpgsqlConnection connection = new(databaseSettings.ConnectionString);
        using Bot bot = new(telegramSettings.BotToken, telegramSettings.AppId, telegramSettings.AppHash, connection);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await engine.ReadLastedMessagesAsync(bot, channel);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Exception: " + ex.Message);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
