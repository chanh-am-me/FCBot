using Infrastructure.Entities;
using Infrastructure.Extensions;
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
    private readonly ITeleBotEngine teleBotEngine;

    public WBotEngine(ITeleBotEngine teleBotEngine)
    {
        this.teleBotEngine = teleBotEngine;
    }

    private const int AppId = 22246669;
    private const string AppHash = "5077609944b128a707af34922df55028";
    private const string BotToken = "7358304134:AAEsNrhGPUSXJ9tDlKA7GgHNvNOr362-FG0";
    private const string ForwardId = " -4538830110";

    public async Task ReadLastedMessagesAsync(ChannelConfig channel)
    {
        using NpgsqlConnection connection = new(@"Host=localhost;Port=5432;Database=telegram-bot;Username=postgres;Password=1;Include Error Detail=true;");
        using Bot bot = new(BotToken, AppId, AppHash, connection);
        List<Message> messages = await bot.GetMessagesById(channel.Id, Enumerable.Range(channel.ReadMessageId, 10));
        foreach (Message message in messages)
        {
            await Console.Out.WriteLineAsync(" Current message read: " + message.MessageId);
            MessageType type = message.Type;
            if (type is MessageType.Unknown)
            {
                continue;
            }

            string content = string.Empty;

            if (message.Type is MessageType.Text)
            {
                content = message.Text ?? string.Empty;
            }

            if (message.Type is MessageType.Photo)
            {
                content = message.Caption ?? message.Text ?? string.Empty;
            }

            if (content == null || RegexExtension.SpamRegex.IsMatch(content))
            {
                channel.ReadMessageId = message.MessageId;
                continue;
            }

            if (IsBobo(content))
            {
                Message forward = await bot.ForwardMessage(ForwardId, channel.Id, message.MessageId);
                await bot.SendTextMessage(ForwardId, "BOBO", replyParameters: forward);
                channel.ReadMessageId = message.MessageId;
                continue;
            }

            if (IsHome(content))
            {
                try
                {
                    Message forward = await bot.ForwardMessage(ForwardId, channel.Id, message.MessageId);
                    await bot.SendTextMessage(ForwardId, "DEV nhà hoặc Kevin", replyParameters: forward);
                    channel.ReadMessageId = message.MessageId;
                }
                catch (Exception)
                {
                    await teleBotEngine.ForwardBackupAsync(ForwardId, channel.Id, message.MessageId, "DEV nhà hoặc Kevin");
                    channel.ReadMessageId = message.MessageId;
                }
                continue;
            }

            if (RegexExtension.SocialRegex.IsMatch(content))
            {
                await bot.ForwardMessage(ForwardId, channel.Id, message.MessageId);
            }


            channel.ReadMessageId = message.MessageId;
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
        if (!balance.Success || (balance.Value != "9.99" && balance.Value != "14.99"))
        {
            return false;
        }

        return true;
    }
}
