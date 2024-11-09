using Infrastructure.Definations;
using Infrastructure.Entities;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Telegram.Bot.Types.Enums;
using WTelegram;
using WTelegram.Types;
using static Infrastructure.Extensions.RegexExtension;

namespace Infrastructure.Engines;

public interface IWBotEngine
{
    Task ReadLastedMessagesAsync(Bot bot, ChannelConfig channel);
}

public class WBotEngine(IOptions<TelegramSettings> telegramOptions) : IWBotEngine
{
    private readonly TelegramSettings telegramSettings = telegramOptions.Value;

    public async Task ReadLastedMessagesAsync(Bot bot, ChannelConfig channel)
    {
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
                IEnumerable<(Message Message, string ForwardId)> forwards = await ForwardsAsync(bot, channel.Id, message.MessageId);
                await SendTextAsync(bot, forwards, "BOBO", false);
                continue;
            }

            if (IsHome(content))
            {
                IEnumerable<(Message Message, string ForwardId)> forwards = await ForwardsAsync(bot, channel.Id, message.MessageId);
                await SendTextAsync(bot, forwards, "DEV nhà hoặc Kevin", false);
                continue;
            }

            if (IsCoinBase(content))
            {
                IEnumerable<(Message Message, string ForwardId)> forwards = await ForwardsAsync(bot, channel.Id, message.MessageId);
                await SendTextAsync(bot, forwards, HtmlMessages.Coinbase, true);
                continue;
            }

            if (IsCrazy(content))
            {
                IEnumerable<(Message Message, string ForwardId)> forwards = await ForwardsAsync(bot, channel.Id, message.MessageId);
                await SendTextAsync(bot, forwards, HtmlMessages.Crazy, true);
                continue;
            }

            if (SocialRegex.IsMatch(content))
            {
                await ForwardsAsync(bot, channel.Id, message.MessageId);
            }

        }
    }

    private async Task<IEnumerable<(Message Message, string ForwardId)>> ForwardsAsync(Bot bot, string channelId, int messagesId)
    {
        List<(Message Message, string ForwardId)> messages = [];
        foreach (string forwardId in telegramSettings.ForwardIds)
        {
            messages.Add((await bot.ForwardMessage(forwardId, channelId, messagesId, messagesId), forwardId));
        }

        return messages;
    }

    private static async Task SendTextAsync(Bot bot, IEnumerable<(Message Message, string ForwardId)> messages, string text, bool isHtml)
    {
        foreach ((Message message, string forwardId) in messages)
        {
            if (isHtml)
            {
                await bot.SendTextMessage(forwardId, text, ParseMode.Html, replyParameters: message);
                continue;
            }

            await bot.SendTextMessage(forwardId, text, replyParameters: message);
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
