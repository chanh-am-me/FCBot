using Infrastructure.Extensions;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.Engines;

public static class BotStatic
{
    private const string BotToken = "7358304134:AAEsNrhGPUSXJ9tDlKA7GgHNvNOr362-FG0";
    private static readonly TelegramBotClient bot = new(BotToken);

    public static void ReceiveMessages()
    {
        bot.OnMessage += OnMessage;
    }
    private static async Task OnMessage(Message msg, UpdateType type)
    {
        string content = string.Empty;
        if (msg.Type is MessageType.Text)
        {
            content = msg.Text ?? string.Empty;
        }

        if (msg.Type is MessageType.Photo)
        {
            content = msg.Caption ?? msg.Text ?? string.Empty;
        }

        if (IsBobo(content))
        {
            await bot.SendTextMessageAsync(msg.Chat, "bobo", replyParameters: msg);
            return;
        }

        if (IsHome(content))
        {
            await bot.SendTextMessageAsync(msg.Chat, "Dev nhà hoặc kevin", replyParameters: msg);
            return;
        }

        if (content == null || RegexExtension.SpamRegex.IsMatch(content))
        {
            return;
        }

        if (RegexExtension.SocialRegex.IsMatch(content))
        {
            await bot.SendTextMessageAsync(msg.Chat, content, replyParameters: msg);
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
