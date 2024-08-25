namespace Infrastructure.Settings;

public class TelegramSettings
{
    public int AppId { get; set; }

    public string AppHash { get; set; } = default!;

    public string BotToken { get; set; } = default!;
}
