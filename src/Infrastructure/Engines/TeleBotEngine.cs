using TL;
using WTelegram;

namespace Infrastructure.Engines;

public interface ITeleBotEngine
{
    Task ForwardBackupAsync(string targetId, string fromId, int messageId, string additionMessgase);
}

public class TeleBotEngine : ITeleBotEngine
{
    private const string BotToken = "7358304134:AAEsNrhGPUSXJ9tDlKA7GgHNvNOr362-FG0";
    private const int AppId = 22246669;
    private const string AppHash = "5077609944b128a707af34922df55028";

    public async Task ForwardBackupAsync(string targetId, string fromId, int messageId, string additionMessgase)
    {
        Client bot = new(Config);
        await bot.LoginUserIfNeeded();
        Contacts_ResolvedPeer resolved = await bot.Contacts_ResolveUsername("DRBTSolana"); // without the @
        if (resolved.Chat is Channel channel)
        {
            InputPeerChat chat = new(long.Parse(targetId));
            UpdatesBase message = await bot.Messages_ForwardMessages(chat, [messageId], [messageId], channel.ToInputPeer());
            //await bot.SendTextMessageAsync(targetId, additionMessgase, replyParameters: message);
        }

    }

    static string Config(string what)
    {
        if (what == "api_id") return AppId.ToString();
        if (what == "api_hash") return AppHash;
        if (what == "phone_number") return "+84359330868";
        if (what == "verification_code") return null; // let WTelegramClient ask the user with a console prompt 
        if (what == "first_name") return "C | Authentic 🏖";      // if sign-up is required
        if (what == "last_name") return null;        // if sign-up is required
        if (what == "password") return "";     // if user has enabled 2FA
        return null;
    }
}
