using Infrastructure.Channels.Requests;
using Infrastructure.Channels.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChannelsController : ControllerBase
{
    private readonly IChannelService channelService;

    public ChannelsController(IChannelService channelService)
    {
        this.channelService = channelService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateChannelRequest request)
        => Ok(await channelService.CreateAsync(request));
}
