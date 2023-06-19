﻿using AutoMapper;
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
        var dt = DateTime.Now.AddHours(-1);
        var trs = await _unitOfWork.GetSet<DbTripRequest>().Where(x => x.Charterer.IsVIP && !x.TripRequestOffers.Any() && x.State == TripRequestStateEnum.Active.GetEnumGuid() && x.DateCreated < dt).ToListAsync(token);
        //var q2 = _unitOfWork.GetSet<DbAccount>().Where(x => x.Email.ToLower().Contains("@tktransfer.ru")).Select(x => x.Id).AsQueryable();
        //var botNotificationsAdmins = await _unitOfWork.GetSet<DbExternalLogin>().Where(x => q2.Contains(x.AccountId) && x.LoginType == ExternalLoginTypeEnum.Telegram).ToListAsync(token);

        foreach(var tr in trs)
        {
            await handleUpdateService.SendMessages(new Bot.Dtos.SendMsgToUserDto
            {
                Link = $"https://nexttripto.ru/TripRequest/{tr.Id}",
                LinkName = "Заказ",
                ChatId = 0,
                NeedMenu = false,
                Message = "На заказ от организации с VIP статусом отсутствуют отклики!"
            });
        }



        return Ok();
    }
}
