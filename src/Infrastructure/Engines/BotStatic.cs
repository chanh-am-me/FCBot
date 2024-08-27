using Infrastructure.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static Infrastructure.Engines.WBotEngine;

namespace Infrastructure.Engines;

public static class BotStatic
{
    private const string BotToken = "6401100360:AAGbOLxtw7o_HxQPjnYgMP-x_v5luG4bvTc";
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
}
