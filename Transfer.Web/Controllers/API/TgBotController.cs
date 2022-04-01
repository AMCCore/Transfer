using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Transfer.Web.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Transfer.Web.Controllers.API;

[ApiController]
public class TgBotController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService, [FromBody] Update update)
    {
        await handleUpdateService.EchoAsync(update);
        return Ok();
    }
}
