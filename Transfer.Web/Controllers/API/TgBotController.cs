using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Transfer.Bot.Dtos;
using Transfer.Common;
using Transfer.Web.Services;

namespace Transfer.Web.Controllers.API;

[ApiController]
[Route("api/[controller]")]
public class TgBotController : ControllerBase
{
    private readonly IMailModule _mailModule;

    public TgBotController(IMailModule mailModule)
    {
        _mailModule = mailModule;
    }

    [HttpPost]
    [Route(nameof(Post))]
    public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService, [FromBody] Update update)
    {
        await handleUpdateService.EchoAsync(update);
        return Ok();
    }

    [HttpPost]
    [Route(nameof(SendMessageToUser))]
    private async Task<IActionResult> SendMessageToUser([FromServices] HandleUpdateService handleUpdateService, [FromBody] SendMsgToUserDto message)
    {
        await handleUpdateService.SendMessages(message);
        return Ok();
    }
}
