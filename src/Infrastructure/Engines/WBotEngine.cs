using Infrastructure.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistent;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Text.RegularExpressions;
using Telegram.Bot.Types.Enums;
using WTelegram;
using WTelegram.Types;

namespace Infrastructure.Engines;

public interface IWBotEngine
{
    Task ReadLastedMessagesAsync(ChannelConfig channel);
}

public class WBotEngine : IWBotEngine
{
    private readonly TelegramSettings telegramSettings;
    private readonly DatabaseSettings databaseSettings;
    public WBotEngine(IOptions<TelegramSettings> telegramOptions, IOptions<DatabaseSettings> databaseOptions)
    {
        telegramSettings = telegramOptions.Value;
        databaseSettings = databaseOptions.Value;
    }

    private const string ForwardId = "-4531896172";

    public async Task ReadLastedMessagesAsync(ChannelConfig channel)
    {
        using NpgsqlConnection connection = new(databaseSettings.ConnectionString);
        using Bot bot = new(telegramSettings.BotToken, telegramSettings.AppId, telegramSettings.AppHash, connection);
        List<Message> messages = await bot.GetMessagesById(channel.Id, Enumerable.Range(channel.ReadMessageId, 10));
        foreach (Message message in messages)
        {
            await Console.Out.WriteLineAsync(" Current message read: " + message.MessageId);
            channel.ReadMessageId = message.MessageId;
            MessageType type = message.Type;
            if (type is MessageType.Unknown)
            {
                return;
            }

            string content = message.Caption ?? message.Text ?? string.Empty;
            if (content == null || RegexExtension.SpamRegex.IsMatch(content))
            {
                continue;
            }

            if (IsBobo(content))
            {
                Message forward = await bot.ForwardMessage(ForwardId, channel.Id, message.MessageId);
                await bot.SendTextMessage(ForwardId, "BOBO", replyParameters: forward);
                continue;
            }

            if (IsHome(content))
            {
                Message forward = await bot.ForwardMessage(ForwardId, channel.Id, message.MessageId, message.MessageId);
                await bot.SendTextMessage(ForwardId, "DEV nhà hoặc Kevin", replyParameters: forward);
                continue;
            }

            if (RegexExtension.SocialRegex.IsMatch(content))
            {
                await bot.ForwardMessage(ForwardId, channel.Id, message.MessageId);
            }

        }
    }

    private static bool IsBobo(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return false;
        }

        Match owner = RegexExtension.FromRegex.Match(content);
        if (!owner.Success)
        {
            return false;
        }

        string walletOwner = owner.Value;
        return walletOwner.StartsWith("5n") && walletOwner.EndsWith("EPs");
    }

    private static bool IsHome(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return false;
        }

        Match supply = RegexExtension.SupplyRegex.Match(content);

        if (!supply.Success || supply.Value != "1,000,000,000")
        {
            return false;
        }

        Match balance = RegexExtension.BalanceRegex.Match(content);
        if (!balance.Success || !balance.Value.Contains(".99"))
        {
            return false;
        }

        if (content.Contains("Description:", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }
}
