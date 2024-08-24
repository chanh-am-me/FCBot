using System.Text.RegularExpressions;

namespace Infrastructure.Extensions;

public static partial class RegexExtension
{
    public static readonly Regex SpamRegex = SpamRg();
    public static readonly Regex SocialRegex = SocialRg();
    public static readonly Regex FromRegex = FromRg();
    public static readonly Regex SupplyRegex = SupplyRg();
    public static readonly Regex BalanceRegex = BalanceRg();

    [GeneratedRegex(@"\|(\s+)?Spam(\s+#)?(\d+)?")]
    private static partial Regex SpamRg();

    [GeneratedRegex("(Website|Telegram|Twitter|Discord)")]
    private static partial Regex SocialRg();

    [GeneratedRegex("(?<=From:\\s)[a-zA-Z0-9.]+")]
    private static partial Regex FromRg();

    [GeneratedRegex("(?<=Supply:\\s)[a-zA-Z0-9.,]+")]
    private static partial Regex SupplyRg();

    [GeneratedRegex("(?<=Balance:\\s)[a-zA-Z0-9.,]+")]
    private static partial Regex BalanceRg();
}
