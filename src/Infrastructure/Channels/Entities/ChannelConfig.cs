using Core.Base;
using Infrastructure.Channels.Requests;
using Infrastructure.Facades.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Channels.Entities;

public class ChannelConfig : BaseEntity
{
    public static ChannelConfig New(CreateChannelRequest request)
    {
        return new()
        {
            SubscribeChannelId = request.ReadChannelId,
            LastedMessageId = request.ReadMessageId,
            ForwardIds = request.ForwardIds,
        };
    }

    public string SubscribeChannelId { get; set; } = default!;

    public int LastedMessageId { get; set; }

    public List<string> ForwardIds { get; set; } = default!;
}

public class ChannelConfigConfigurations : IEntityTypeConfiguration<ChannelConfig>
{
    public void Configure(EntityTypeBuilder<ChannelConfig> builder)
    {
        builder.HasBaseEntity().UnderscoreTable();
    }
}
