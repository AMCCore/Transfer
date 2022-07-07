using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Bl.Dto.TripRequest;
using Transfer.Common;
using Transfer.Common.Enums;
using Transfer.Common.Enums.AccessRights;
using Transfer.Common.Enums.States;
using Transfer.Common.Extensions;
using Transfer.Common.Settings;
using Transfer.Dal.Entities;
using Transfer.Web.Extensions;
using Transfer.Web.Models;
using Transfer.Web.Models.TripRequest;
using Transfer.Web.Services;

namespace Transfer.Web.Controllers;

[Authorize]
public sealed class TripRequestController : BaseStateController
{
    const string errMsgName = "errMsg";

    private HandleUpdateService _handleUpdateService;

    public TripRequestController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<TripRequestController> logger, IMapper mapper, HandleUpdateService handleUpdateService) : base(transferSettings, unitOfWork, logger, mapper)
    {
        _handleUpdateService = handleUpdateService;
    }

    private async Task<RequestSearchFilter> GetDataFromDb(RequestSearchFilter filter = null)
    {
        filter ??= new RequestSearchFilter(new List<TripRequestSearchResultItem>(), TransferSettings.TablePageSize);
        var query = UnitOfWork.GetSet<DbTripRequest>().Where(x => !x.IsDeleted).OrderBy(x => x.DateCreated).AsQueryable();

        if (filter.OrderByName)
        {
            query = query.OrderBy(x => x.СhartererName).ThenBy(x => x.Charterer.Name);
        }

        if (filter.OrderByRating)
        {
            //query = query.OrderBy(x => x.Rating).ThenBy(x => x.Name);
        }

        if (filter.OrderByChecked)
        {
            //query = query.OrderBy(x => x.Rating).ThenBy(x => x.Name);
        }

        if (filter.OrderByChild)
        {
            query = query.OrderByDescending(x => x.TripOptions.Any(y => y.TripOptionId == TripOptionsEnum.ChildTrip.GetEnumGuid()))
                .ThenBy(x => x.СhartererName)
                .ThenBy(x => x.Charterer.Name);
        }

        var totalCount = await query.CountAsync(CancellationToken.None);
        var entity = await query.Skip(filter.StartRecord)
            .Take(filter.PageSize).ToListAsync(CancellationToken.None);

        filter.Results = new CommonPagedList<TripRequestSearchResultItem>(
            entity.Select(ss => Mapper.Map<TripRequestSearchResultItem>(ss)).ToList(),
            filter.PageNumber, filter.PageSize, totalCount);

        return filter;
    }

    [Route("TripRequests")]
    [HttpGet]
    public async Task<IActionResult> Search()
    {
        var result = await GetDataFromDb();
        return View(result);
    }

    [HttpPost]
    [Route("TripRequests")]
    public async Task<IActionResult> Search(RequestSearchFilter filter)
    {
        var result = await GetDataFromDb(filter);

        return PartialView("SearchResults", result);
    }

    [HttpGet]
    [Route("TripRequest/{requestId}")]
    public async Task<IActionResult> TripRequestShow(Guid requestId)
    {
        var entity = await UnitOfWork.GetSet<DbTripRequest>().FirstOrDefaultAsync(ss => ss.Id == requestId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        var model = Mapper.Map<TripRequestWithOffersDto>(entity);
        SetNextStates(model);

        model.Offers = await UnitOfWork.GetSet<DbTripRequestOffer>().Where(x => !x.IsDeleted && x.TripRequestId == requestId).Include(x => x.Carrier).Select(x => Mapper.Map<TripRequestOfferSearchResultItem>(x)).ToListAsync();
        return View("Show", model);
    }

    [HttpGet]
    [Route("TripRequest/Edit/{requestId}")]
    public async Task<IActionResult> TripRequestEdit(Guid requestId)
    {
        var entity = await UnitOfWork.GetSet<DbTripRequest>().FirstOrDefaultAsync(ss => ss.Id == requestId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        var model = Mapper.Map<TripRequestDto>(entity);
        SetNextStates(model);
        return View("Save", model);
    }

    [HttpGet]
    [Route("TripRequest/Delete/{requestId}")]
    public async Task<IActionResult> TripRequestDelete(Guid requestId)
    {
        var entity = await UnitOfWork.GetSet<DbTripRequest>().FirstOrDefaultAsync(ss => ss.Id == requestId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        entity.IsDeleted = true;

        await UnitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Search));
    }

    [HttpGet]
    [Route("TripRequest/New")]
    public IActionResult NewTripRequest()
    {
        return View("Save", new TripRequestDto
        {
            TripDate = DateTime.Now.AddDays(1).ChangeTime(9, 0),
            PaymentType = (int)PaymentTypeEnum.Card
        });
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("MakeOffer/{requestId}")]
    public async Task<IActionResult> MakeOffer(Guid requestId)
    {
        var replay = await UnitOfWork.GetSet<DbTripRequestReplay>().Include(x => x.TripRequest).FirstOrDefaultAsync(x => x.Id == requestId);
        if (replay == null || replay.IsDeleted)
        {
            TempData[errMsgName] = "Поездка не найдена";
            return RedirectToAction(nameof(MakeOfferError));
            //return NotFound();
        }
        if (replay.DateValid <= DateTime.Now)
        {
            TempData[errMsgName] = "Поездка уже прошла";
            return RedirectToAction(nameof(MakeOfferError));
            //return BadRequest("Times up");
        }
        if (await UnitOfWork.GetSet<DbTripRequestOffer>().AnyAsync(x => x.TripRequestId == replay.TripRequestId && x.CarrierId == replay.CarrierId))
        {
            TempData[errMsgName] = "Ваше предложение уже учтено";
            return RedirectToAction(nameof(MakeOfferError));
            //return BadRequest("Already offered");
        }

        var model = Mapper.Map<TripRequestOfferDto>(replay.TripRequest);
        SetNextStates(model);
        model.CarrierId = replay.CarrierId;
        return View("MakeOffer", model);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("MakeOffer/Save")]
    [AllowAnonymous]
    public async Task<IActionResult> OfferSave([FromForm] TripRequestOfferDto model)
    {
        if (model.Id.IsNullOrEmpty())
        {
            return BadRequest();
        }
        if (model.Amount < 1)
        {
            ViewBag.ErrorMsg = "Одно или несколько полей не заполнены";
            return View("MakeOffer", model);
        }

        var req = await UnitOfWork.GetSet<DbTripRequest>().FirstOrDefaultAsync(x => x.Id == model.Id);

        var offer = new DbTripRequestOffer
        {
            CarrierId = model.CarrierId,
            Amount = model.Amount,
            Comment = model.Comment,
            TripRequestId = model.Id
        };

        await UnitOfWork.AddEntityAsync(offer);

        return RedirectToAction(nameof(MakeOfferSuccess));
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("TripRequest/Save")]
    public async Task<IActionResult> Save([FromForm] TripRequestDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Одно или несколько полей не заполнены";
            return View("Save", model);
        }

        if (model.Id.IsNullOrEmpty())
        {
            model.Id = Guid.NewGuid();
            var entity = Mapper.Map<DbTripRequest>(model);
            if (string.IsNullOrWhiteSpace(entity.ContactFio))
            {
                entity.ContactFio = entity.СhartererName;
            }

            await UnitOfWork.AddEntityAsync(entity, CancellationToken.None);

            await UnitOfWork.AddEntityAsync(new DbTripRequestIdentifier { 
                TripRequestId = model.Id
            }, CancellationToken.None);

            await SetTripOptions(entity, model);
            await SetTripRegions(entity, model);

            var appropriateOrgIds = await UnitOfWork.GetSet<DbOrganisation>().Where(x => !x.IsDeleted)
                .Where(x => x.WorkingArea.Any(wa => wa.RegionId == entity.RegionFromId.Value) || x.WorkingArea.Any(wa => wa.RegionId == entity.RegionToId.Value)).Select(x => x.Id).ToListAsync(CancellationToken.None);
            foreach (var orgId in appropriateOrgIds)
            {
                await UnitOfWork.AddEntityAsync(new DbTripRequestReplay
                {
                    TripRequestId = entity.Id,
                    CarrierId = orgId,
                    DateValid = DateTime.Now.AddDays(4),
                }, CancellationToken.None);
            }

            await SendReplaysToUsers(entity.Id);
            await UnitOfWork.AddToHistoryLog(entity, "Создание запроса на перевозку");
        }
        else
        {
            var entity = await UnitOfWork.GetSet<DbTripRequest>().FirstOrDefaultAsync(ss => ss.Id == model.Id, CancellationToken.None);

            if (entity.LastUpdateTick != model.LastUpdateTick)
                throw new InvalidOperationException();

            Mapper.Map(model, entity);
            await SetTripOptions(entity, model);
            await SetTripRegions(entity, model);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);
            await UnitOfWork.AddToHistoryLog(entity, "Запроса на перевозку изменён");
        }

        return RedirectToAction(nameof(TripRequestEdit), new { requestId = model.Id });
    }

    private async Task SetTripOptions(DbTripRequest entity, TripRequestDto model)
    {
        foreach (var to in await UnitOfWork.GetSet<DbTripRequestOption>().Where(x => x.TripRequestId == entity.Id).ToListAsync(CancellationToken.None))
        {
            await UnitOfWork.DeleteAsync(to, CancellationToken.None);
        }
        if (model.ChildTrip)
        {
            await UnitOfWork.AddEntityAsync(new DbTripRequestOption
            {
                Id = Guid.NewGuid(),
                TripRequestId = entity.Id,
                TripOptionId = TripOptionsEnum.ChildTrip.GetEnumGuid()
            }, CancellationToken.None);
        }
        if (model.StandTrip)
        {
            await UnitOfWork.AddEntityAsync(new DbTripRequestOption
            {
                Id = Guid.NewGuid(),
                TripRequestId = entity.Id,
                TripOptionId = TripOptionsEnum.IdleTrip.GetEnumGuid()
            }, CancellationToken.None);
        }
        if (model.PaymentType == (int)PaymentTypeEnum.Card)
        {
            await UnitOfWork.AddEntityAsync(new DbTripRequestOption
            {
                Id = Guid.NewGuid(),
                TripRequestId = entity.Id,
                TripOptionId = TripOptionsEnum.CardPayment.GetEnumGuid()
            }, CancellationToken.None);

        }
        else if (model.PaymentType == (int)PaymentTypeEnum.Cash)
        {
            await UnitOfWork.AddEntityAsync(new DbTripRequestOption
            {
                Id = Guid.NewGuid(),
                TripRequestId = entity.Id,
                TripOptionId = TripOptionsEnum.CashPayment.GetEnumGuid()
            }, CancellationToken.None);
        }
    }

    public override IDictionary<Guid, string> GetPossibleStatets(Guid currentState)
    {
        var res = new Dictionary<Guid, string>();
        var isTripRequestAdmin = HasRight(TripRequestRights.TripRequestAdmin);


        if (currentState == TripRequestStateEnum.New.GetEnumGuid())
        {
            if (isTripRequestAdmin)
            {
                res.Add(TripRequestStateEnum.ProposalsComplete.GetEnumGuid(), "Отменить");
                res.Add(TripRequestStateEnum.ProposalsComplete.GetEnumGuid(), "Закрыть сбор предложений");
            }
        }
        return res;
    }

    /// <summary>
    /// Установка регионов 
    /// </summary>
    private async Task SetTripRegions(DbTripRequest entity, TripRequestDto model)
    {
        if (model.RegionFromId.IsNullOrEmpty() && !string.IsNullOrWhiteSpace(model.RegionFromName))
        {
            var reg = await GetOrCreateRegion(model.RegionFromName);
            entity.RegionFromId = reg.Id;
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        }
        if (model.RegionToId.IsNullOrEmpty() && !string.IsNullOrWhiteSpace(model.RegionToName))
        {
            var reg = await GetOrCreateRegion(model.RegionToName);
            entity.RegionToId = reg.Id;
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }

    private async Task<DbRegion> GetOrCreateRegion(string regionName)
    {
        var reg = await UnitOfWork.GetSet<DbRegion>().FirstOrDefaultAsync(ss => ss.Name.ToLower().Contains(regionName.ToLower()), CancellationToken.None);
        if (reg == null)
        {
            reg = new DbRegion
            {
                Name = regionName
            };
            await UnitOfWork.AddEntityAsync(reg, CancellationToken.None);
        }
        return reg;
    }

    private async Task SendReplaysToUsers(Guid tripRequestId)
    {
        var replays = await UnitOfWork.GetSet<DbTripRequestReplay>().Where(x => x.TripRequestId == tripRequestId && !x.IsDeleted && !x.Carrier.IsDeleted)
            .Select(x => x.Id).ToListAsync();

        var trip = await UnitOfWork.GetSet<DbTripRequest>().Include(x => x.Identifiers).FirstAsync(x => x.Id == tripRequestId);

        foreach (var replay in replays)
        {
            var orgUsers = await UnitOfWork.GetSet<DbTripRequestReplay>().Where(x => x.Id == replay).Select(x => x.Carrier).SelectMany(x => x.Accounts.Where(a => !a.Account.IsDeleted).Select(a => a.Account))
                .SelectMany(x => x.ExternalLogins.Where(a => !a.IsDeleted && a.LoginType == ExternalLoginTypeEnum.Telegram)).ToListAsync();

            foreach (var orgUser in orgUsers)
            {
                if (long.TryParse(orgUser.Value, out long chatId))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"Новый заказ №{trip.Identifiers.Select(x => x.Identifier).FirstOrDefault()}");
                    sb.AppendLine($"Заказчик: {(!trip.ChartererId.IsNullOrEmpty() ? trip.Charterer.Name : trip.СhartererName)}");
                    sb.AppendLine($"Дата отправления: {trip.TripDate:dd.MM.yyyy HH:mm}");
                    sb.AppendLine($"Место отправления: {trip.AddressFrom}");
                    sb.AppendLine($"Место прибытия: {trip.AddressTo}");
                    sb.AppendLine($"Кол-во пассажиров: {trip.Passengers}");
                    sb.AppendLine();
                    sb.AppendLine("Чтобы откликнуться перейдите по ссылке");
                    sb.AppendLine($"https://nexttripto.ru/MakeOffer/{replay}");


                    _handleUpdateService?.SendMessages(new Bot.Dtos.SendMsgToUserDto
                    {
                        ChatId = chatId,
                        Message = sb.ToString()
                        //Message = $"Новый заказ на: {trip.TripDate:dd.MM.yyyy HH:mm}\nОткуда: {trip.AddressFrom}\nКуда:{trip.AddressTo}\nЧтобы откликнуться перейдите по ссылке\nhttps://nexttripto.ru/MakeOffer/{replay}",
                        //Link = $"https://nexttripto.ru/MakeOffer/{replay}",
                        //LinkName = "Откликнуться"
                    });
                }
            }
        }
    }
    
    [AllowAnonymous]
    [HttpGet]
    [Route("MakeOffer/Success")]
    public IActionResult MakeOfferSuccess()
    {
        return View("Success");
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("MakeOffer/Error")]
    public async Task<IActionResult> MakeOfferError()
    {
        var msg = TempData[errMsgName] as string;
        if(string.IsNullOrWhiteSpace(msg))
        {
            return await RedirectToHomeAsync();
        }
        ViewBag.Message = msg;
        return View("Error");
    }
}