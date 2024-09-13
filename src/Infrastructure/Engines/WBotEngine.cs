using Infrastructure.Definations;
using Infrastructure.Entities;
using Infrastructure.Persistent;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Text.RegularExpressions;
using Telegram.Bot.Types.Enums;
using WTelegram;
using WTelegram.Types;
using static Infrastructure.Extensions.RegexExtension;

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

    private const string ForwardId = "-4587788294";

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
            if (content == null || SpamRegex.IsMatch(content))
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

            if (IsCoinBase(content))
            {
                Message forward = await bot.ForwardMessage(ForwardId, channel.Id, message.MessageId, message.MessageId);
                await bot.SendTextMessage(ForwardId, HtmlMessages.Coinbase, ParseMode.Html, replyParameters: forward);
            }

            if (IsCrazy(content))
            {
                Message forward = await bot.ForwardMessage(ForwardId, channel.Id, message.MessageId, message.MessageId);
                await bot.SendTextMessage(ForwardId, HtmlMessages.Crazy, ParseMode.Html, replyParameters: forward);
            }

            if (SocialRegex.IsMatch(content))
            {
                await bot.ForwardMessage(ForwardId, channel.Id, message.MessageId);
            }

        }
    }

    public static bool IsBobo(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return false;
        }

        Match owner = FromRegex.Match(content);
        if (!owner.Success || !owner.Value.StartsWith("5n") || !owner.Value.EndsWith("EPs"))
        {
            return false;
        }

        if (!SocialRegex.IsMatch(content))
        {
            return false;
        }

        return true;
    }

    public static bool IsHome(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return false;
        }

        Match supply = SupplyRegex.Match(content);

        if (!supply.Success || supply.Value != "1,000,000,000")
        {
            return false;
        }

        Match balance = BalanceRegex.Match(content);
        if (!balance.Success || !XX99BalanceRegex.IsMatch(balance.Value))
        {
            return false;
        }

        if (content.Contains("Description:", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        Match from = FromRegex.Match(content);

        if (!from.Success || string.IsNullOrEmpty(from.Value))
        {
            return false;
        }

        return true;
    }

    public static bool IsCoinBase(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return false;
        }

        Match supply = SupplyRegex.Match(content);

        if (!supply.Success || supply.Value != "1,000,000,000")
        {
            return false;
        }

        Match balance = BalanceRegex.Match(content);

        if (!balance.Success || string.IsNullOrEmpty(balance.Value))
        {
            return false;
        }

        Match from = FromRegex.Match(content);

        if (!from.Success || string.IsNullOrEmpty(from.Value) || !from.Value.Contains("Coinbase"))
        {
            return false;
        }

        if (!content.Contains("Description:", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        Match twitterHashLink = TwitterHasLinkRegex.Match(content);

        if (!twitterHashLink.Success || string.IsNullOrEmpty(twitterHashLink.Value))
        {
            return false;
        }

        Match teleHashLink = TeleHasLinkRegex.Match(content);

        if (!teleHashLink.Success || string.IsNullOrEmpty(teleHashLink.Value))
        {
            return false;
        }

        Match websiteHashLink = WebsiteRegex.Match(content);

        if (!websiteHashLink.Success || string.IsNullOrEmpty(websiteHashLink.Value))
        {
            return false;
        }

        return true;
    }

    public static bool IsCrazy(string content)
    {

        if (string.IsNullOrEmpty(content))
        {
            return false;
        }

        Match supply = SupplyRegex.Match(content);

        if (!supply.Success || !supply.Value.Contains("000,000,000,000"))
        {
            return false;
        }

        Match balance = BalanceRegex.Match(content);

        if (!balance.Success || string.IsNullOrEmpty(balance.Value) /*|| (!balance.Value.Contains("50") && !balance.Value.Contains("60"))*/)
        {
            return false;
        }

        Match from = FromRegex.Match(content);

        if (!from.Success || string.IsNullOrEmpty(from.Value) || !from.Value.Contains("Kucoin", StringComparison.OrdinalIgnoreCase))
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
