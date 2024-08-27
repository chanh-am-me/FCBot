namespace Infrastructure.Entities;

public class ChannelConfig
{
    public static readonly ChannelConfig Default = GetDefault();

    public string Id { get; set; } = default!;

    public int ReadMessageId { get; set; }

    private static ChannelConfig GetDefault()
    {
        return new()
        {
            Id = "@DRBTSolana",
<<<<<<< Updated upstream
            ReadMessageId = 498951,
=======
            ReadMessageId = 499734,
>>>>>>> Stashed changes
        };
    }
}
