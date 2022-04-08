using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Transfer.Web.Services;

namespace Transfer.Web.Controllers.API;

[ApiController]
[Route("api/[controller]")]
public class TgBotController : ControllerBase
{
    #if DEBUG

    [HttpPost]
    [Route(nameof(Post))]
    public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService, [FromBody] Update update)
    {
        await handleUpdateService.EchoAsync(update);
        return Ok();
    }

    #endif
}
