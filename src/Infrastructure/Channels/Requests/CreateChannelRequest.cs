namespace Infrastructure.Channels.Requests;

public class CreateChannelRequest
{
    public string ReadChannelId { get; set; } = default!;

    public int ReadMessageId { get; set; }

    public List<string> ForwardIds { get; set; } = default!;
}
