using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;
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

    [HttpGet]
    [Route(nameof(SendTestEmail))]
    public async Task<IActionResult> SendTestEmail()
    {
        await _mailModule.SendEmailPlainTextAsync($"<h2>Приветствуем вас</h2></br></br>Поступил запрос на подключение к телеграм боту.</br>Чтобы подтвердить перейдите по ссылке</br><a href=\"https://nexttripto.ru/TgAccept/df50d6f9-65b3-453e-a292-7b49da2fd01f\" target=\"self\">перейти</a>", "Запрос на подтверждение использования телеграм бота", "markin.nikolai@gmail.com");
        return Ok();
    }
}
