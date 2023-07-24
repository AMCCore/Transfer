using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using Transfer.Common;
using Transfer.Web.Services;
using Transfer.Dal.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Transfer.Common.Enums;
using Transfer.Common.Enums.States;
using Transfer.Common.Extensions;
using System;
using System.Text;

namespace Transfer.Web.Controllers.API;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CronController : ControllerBase
{
    private readonly IWebHostEnvironment _appEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CronController(IWebHostEnvironment appEnvironment, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _appEnvironment = appEnvironment;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Оповещение менеджеров о заказе от VIP организации без отклика
    /// </summary>
    [AllowAnonymous]
    [HttpGet]
    [Route(nameof(VipTripRequestNoOfferManagerRemind))]
    public async Task<IActionResult> VipTripRequestNoOfferManagerRemind([FromServices] HandleUpdateService handleUpdateService, CancellationToken token = default)
    {
        var dt = DateTime.Now.AddHours(-2);
        var dt2 = DateTime.Now.AddDays(1);
        var trs = await _unitOfWork.GetSet<DbTripRequest>().Where(x => (x.Charterer.IsVIP || x.OrgCreator.IsVIP) && !x.TripRequestOffers.Any() && x.State == TripRequestStateEnum.Active.GetEnumGuid() && x.DateCreated < dt && x.TripDate >= dt2).OrderBy(x => x.DateCreated).Select(x => new { x.Id, x.СhartererName, x.ChartererId, Identifier = x.Identifiers.Select(y =>  y.Identifier).FirstOrDefault() }).ToListAsync(token);
        //var q2 = _unitOfWork.GetSet<DbAccount>().Where(x => x.Email.ToLower().Contains("@tktransfer.ru")).Select(x => x.Id).AsQueryable();
        //var botNotificationsAdmins = await _unitOfWork.GetSet<DbExternalLogin>().Where(x => q2.Contains(x.AccountId) && x.LoginType == ExternalLoginTypeEnum.Telegram).ToListAsync(token);

        var sb = new StringBuilder();

        foreach(var tr in trs)
        {
            sb.AppendLine($"Организацией с VIP статусом (<a href='https://nexttripto.ru/Carrier/{tr.ChartererId}'>{tr.СhartererName}</a>) создан новый заказ (<a href='https://nexttripto.ru/TripRequest/{tr.Id}'>{tr.Identifier}</a>)");
            sb.AppendLine();

            if(sb.Length >= 3072)
            {
                await handleUpdateService.SendMessages(new Bot.Dtos.SendMsgToUserDto
                {
                    ChatId = -1001842218707,
                    NeedMenu = false,
                    IsHtmlLike = true,
                    Message = sb.ToString()
                });
                sb = new StringBuilder();
            }
        }

        if (sb.Length > 0)
        {
            await handleUpdateService.SendMessages(new Bot.Dtos.SendMsgToUserDto
            {
                ChatId = -1001842218707,
                NeedMenu = false,
                IsHtmlLike = true,
                Message = sb.ToString()
            });
        }

        return Ok();
    }
}
