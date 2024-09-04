using Infrastructure.Channels.Entities;
using Infrastructure.Channels.Requests;
using Infrastructure.Persistents.Repositories;
using static Infrastructure.Channels.Entities.ChannelConfig;

namespace Infrastructure.Channels.Services;

public interface IChannelService
{
    Task<ChannelConfig> CreateAsync(CreateChannelRequest request);
}

public class ChannelService : IChannelService
{
    private readonly IRepositoryWrapper repositoryWrapper;

    public ChannelService(IRepositoryWrapper repositoryWrapper)
    {
        this.repositoryWrapper = repositoryWrapper;
    }

    public async Task<ChannelConfig> CreateAsync(CreateChannelRequest request)
    {
        ChannelConfig channel = New(request);
        return await repositoryWrapper.Repository<ChannelConfig>().AddAsync(channel);
    }
}
