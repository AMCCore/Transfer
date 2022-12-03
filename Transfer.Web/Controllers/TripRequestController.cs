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
using Transfer.Bl.Dto;
using Transfer.Bl.Dto.TripRequest;
using Transfer.Common;
using Transfer.Common.Enums;
using Transfer.Common.Enums.AccessRights;
using Transfer.Common.Enums.States;
using Transfer.Common.Extensions;
using Transfer.Common.Settings;
using Transfer.Dal.Entities;
using Transfer.Dal.Helpers;
using Transfer.Web.Extensions;
using Transfer.Web.Models;
using Transfer.Web.Models.Enums;
using Transfer.Web.Models.TripRequest;
using Transfer.Web.Services;

namespace Transfer.Web.Controllers;

[Authorize]
public sealed class TripRequestController : BaseStateController
{
    const string errMsgName = "errMsg";

    private HandleUpdateService _handleUpdateService;

#if !DEBUG

    public TripRequestController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<TripRequestController> logger, IMapper mapper, HandleUpdateService handleUpdateService) : base(transferSettings, unitOfWork, logger, mapper)
    {
        _handleUpdateService = handleUpdateService;
    }

#else

    public TripRequestController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<TripRequestController> logger, IMapper mapper) : base(transferSettings, unitOfWork, logger, mapper)
    {
    }

#endif

    private async Task<RequestSearchFilter> GetDataFromDb(RequestSearchFilter filter = null)
    {
        await UnitOfWork.TripRequestStateRegulate();

        filter ??= new RequestSearchFilter(new List<TripRequestSearchResultItem>(), TransferSettings.TablePageSize);
        var query = UnitOfWork.GetSet<DbTripRequest>().Where(x => !x.IsDeleted).AsQueryable();

        if (filter.State == (int)TripRequestSearchStateEnum.StateNew)
        {
            query = query.Where(x => x.State == TripRequestStateEnum.Active);
        }
        else if (filter.State == (int)TripRequestSearchStateEnum.StateComplete)
        {
            query = query.Where(x => x.State == TripRequestStateEnum.Completed);
        }
        else if (filter.State == (int)TripRequestSearchStateEnum.StateOnair)
        {
            query = query.Where(x => x.State == TripRequestStateEnum.CarrierSelected);
        }
        else if (filter.State == (int)TripRequestSearchStateEnum.StateCanceled)
        {
            query = query.Where(x => x.State == TripRequestStateEnum.Canceled || x.State == TripRequestStateEnum.Overdue || x.State == TripRequestStateEnum.Archived);
        }

        if (filter.OrderBy == (int)TripRequestSearchOrderEnum.OrderByDateStartAsc)
        {
            query = query.OrderBy(x => x.TripDate).ThenBy(x => x.DateCreated);
        }
        else if (filter.OrderBy == (int)TripRequestSearchOrderEnum.OrderByDateStartDesc)
        {
            query = query.OrderByDescending(x => x.TripDate).ThenBy(x => x.DateCreated);
        }
        else if (filter.OrderBy == (int)TripRequestSearchOrderEnum.OrderByDateCreatedAsc)
        {
            query = query.OrderBy(x => x.DateCreated);
        }
        else if (filter.OrderBy == (int)TripRequestSearchOrderEnum.OrderByDateCreatedDesc)
        {
            query = query.OrderByDescending(x => x.DateCreated);
        }

        //query = query.OrderBy(x => x.СhartererName).ThenBy(x => x.Charterer.Name);

        //if (filter.OrderByChild)
        //{
        //    query = query.OrderByDescending(x => x.TripOptions.Any(y => y.TripOptionId == TripOptionsEnum.ChildTrip.GetEnumGuid()))
        //        .ThenBy(x => x.СhartererName)
        //        .ThenBy(x => x.Charterer.Name);
        //}

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
        await SetNextStates(model);

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
        await SetNextStates(model);
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

            await UnitOfWork.AddEntityAsync(new DbTripRequestIdentifier
            {
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
                }, CancellationToken.None);
            }

            await SendReplaysToUsers(entity.Id);
            await SendReplaysToWatchers(entity.Id);
            await UnitOfWork.AddToHistoryLog(entity, "Создание запроса на перевозку");
        }
        else
        {
            throw new NotSupportedException();

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

    public override async Task<ICollection<NextStateDto>> GetPossibleStatets(Guid currentState)
    {
        var q = UnitOfWork.GetSet<DbStateMachineState>().Where(x => x.StateMachine == StateMachineEnum.TripRequest);
        q = q.Where(x => x.StateFrom == currentState);
        q = q.Where(x => !x.UseBySystem);

        return await q.Select(x => new NextStateDto { ButtonName = x.ButtonName, ConfirmText = x.ConfirmText, NextStateId = x.StateTo, NeedSaveButton = false }).ToListAsync();
    }

    /// <summary>
    /// Установка регионов 
    /// </summary>
    private async Task SetTripRegions(DbTripRequest entity, TripRequestDto model)
    {
        if (model.RegionFromId.IsNullOrEmpty() && !string.IsNullOrWhiteSpace(model.RegionFromName))
        {
            var reg = await GetOrCreateRegion(model.RegionFromName);
            entity.RegionFromId = reg?.Id;
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        }
        if (model.RegionToId.IsNullOrEmpty() && !string.IsNullOrWhiteSpace(model.RegionToName))
        {
            var reg = await GetOrCreateRegion(model.RegionToName);
            entity.RegionToId = reg?.Id;
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }

    private async Task<DbRegion> GetOrCreateRegion(string regionName)
    {
        var reg = await UnitOfWork.GetSet<DbRegion>().FirstOrDefaultAsync(ss => ss.Name.ToLower().Contains(regionName.ToLower()), CancellationToken.None);
        if (reg == null)
        {
            //reg = new DbRegion
            //{
            //    Name = regionName
            //};
            //await UnitOfWork.AddEntityAsync(reg, CancellationToken.None);
        }
        return reg;
    }

    [HttpGet]
    [Route("TripRequest/StateChange")]
    public async Task<IActionResult> TripRequestStateChange(Guid requestId, Guid stateId)
    {
        var entity = await UnitOfWork.GetSet<DbTripRequest>().Where(ss => ss.Id == requestId && !ss.IsDeleted).FirstOrDefaultAsync();
        if (entity == null)
            throw new ArgumentNullException(nameof(requestId));

        var q = UnitOfWork.GetSet<DbStateMachineState>().Where(x => x.StateMachine == StateMachineEnum.TripRequest);
        q = q.Where(x => x.StateFrom == entity.State.GetEnumGuid());
        q = q.Where(x => x.StateTo == stateId);
        if (await q.CountAsync() != 1)
            throw new ArgumentOutOfRangeException();

        var nextState = await q.FirstOrDefaultAsync();

        if (Moduls.Security.Current.HasRightForSomeOrganisation(TripRequestRights.TripRequestAdmin))
        {
            entity.State = GuidEnumConverter<TripRequestStateEnum>.ConvertToEnum(nextState.StateTo);
            await UnitOfWork.SaveChangesAsync();
            await UnitOfWork.AddToHistoryLog(entity, "Статус запроса на перевозку изменён", nextState.Description);

            if (nextState.StateTo == TripRequestStateEnum.Canceled.GetEnumGuid())
            {
                await SendChancelOferToUsers(entity.Id);
            }

            return RedirectToAction(nameof(Search));
        }

        return RedirectToHome();
    }

    [HttpGet]
    [Route("TripRequest/{requestId}/CarrierChoose/{offerId}")]
    public async Task<IActionResult> TripRequestSetCarrierChoosen(Guid requestId, Guid offerId)
    {
        var entity = await UnitOfWork.GetSet<DbTripRequestOffer>().Where(ss => ss.Id == offerId && !ss.IsDeleted && ss.TripRequestId == requestId).FirstOrDefaultAsync();
        if (entity == null)
            throw new ArgumentNullException(nameof(offerId));

        var trip = entity.TripRequest;

        if (Moduls.Security.Current.HasRightForSomeOrganisation(TripRequestRights.TripRequestAdmin) && trip.State == TripRequestStateEnum.Active)
        {
            entity.Chosen = true;
            trip.State = TripRequestStateEnum.CarrierSelected;
            await UnitOfWork.SaveChangesAsync();
            await UnitOfWork.AddToHistoryLog(trip, "Перевозчик выбран", $"Перевозчик: {entity.Carrier.Name}({entity.Carrier.Id}), сумма предложения: {entity.Amount}");

            await SendChoosenOferToUsers(entity.Id);
            await SendUnChoosenOferToUsers(trip.Id);


            return RedirectToAction(nameof(TripRequestShow), new { requestId = entity.TripRequestId });
        }

        return RedirectToHome();
    }

    #region Make offer

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
        if (string.IsNullOrWhiteSpace(msg))
        {
            return await RedirectToHomeAsync();
        }
        ViewBag.Message = msg;
        return View("Error");
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("MakeOffer/{requestId}")]
    public async Task<IActionResult> MakeOffer(Guid requestId)
    {
        var replay = await UnitOfWork.GetSet<DbTripRequestReplay>().Include(x => x.TripRequest).FirstOrDefaultAsync(x => x.Id == requestId);
        if (replay == null || replay.IsDeleted || replay.TripRequest.State == TripRequestStateEnum.Archived)
        {
            TempData[errMsgName] = "Поездка не найдена";
            return RedirectToAction(nameof(MakeOfferError));
        }
        if (replay.TripRequest.TripDate <= DateTime.Now || replay.TripRequest.State == TripRequestStateEnum.Completed)
        {
            TempData[errMsgName] = "Поездка уже прошла";
            return RedirectToAction(nameof(MakeOfferError));
        }
        if (replay.TripRequest.State != TripRequestStateEnum.Active)
        {
            TempData[errMsgName] = "Исполнитель на данную поездку уже выбран";
            return RedirectToAction(nameof(MakeOfferError));
        }
        if (await UnitOfWork.GetSet<DbTripRequestOffer>().AnyAsync(x => x.TripRequestId == replay.TripRequestId && x.CarrierId == replay.CarrierId))
        {
            TempData[errMsgName] = "Ваше предложение уже учтено";
            return RedirectToAction(nameof(MakeOfferError));
        }

        var model = Mapper.Map<TripRequestOfferDto>(replay.TripRequest);
        await SetNextStates(model);
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

    #endregion

    #region Telegram send

    /// <summary>
    /// Уведомление о новом заказе в ситеме
    /// </summary>
    /// <param name="tripRequestId"></param>
    /// <returns></returns>
    private async Task SendReplaysToUsers(Guid tripRequestId)
    {
        var replays = await UnitOfWork.GetSet<DbTripRequestReplay>().Where(x => x.TripRequestId == tripRequestId && !x.IsDeleted && !x.Carrier.IsDeleted)
            .Select(x => x.Id).ToListAsync();

        var trip = await UnitOfWork.GetSet<DbTripRequest>().FirstAsync(x => x.Id == tripRequestId);

        foreach (var replay in replays)
        {
            var orgUsers = await UnitOfWork.GetSet<DbTripRequestReplay>().Where(x => x.Id == replay).Select(x => x.Carrier).SelectMany(x => x.Accounts.Where(a => !a.Account.IsDeleted && a.AccountType == OrganisationAccountTypeEnum.Operator || a.AccountType == OrganisationAccountTypeEnum.Director).Select(a => a.Account))
                .SelectMany(x => x.ExternalLogins.Where(a => !a.IsDeleted && a.LoginType == ExternalLoginTypeEnum.Telegram)).ToListAsync();

            foreach (var orgUser in orgUsers.DistinctBy(x => x.Value).ToList())
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

    /// <summary>
    /// Уведомление о новом заказе в ситеме
    /// </summary>
    /// <param name="tripRequestId"></param>
    /// <returns></returns>
    private async Task SendReplaysToWatchers(Guid tripRequestId)
    {
        var trip = await UnitOfWork.GetSet<DbTripRequest>().FirstAsync(x => x.Id == tripRequestId);

        var botNotificationsAdmins = await UnitOfWork.GetSet<DbAccount>().Where(x => x.AccountRights.Any(y => y.RightId == AdminAccessRights.BotNotifications.GetEnumGuid()))
            .SelectMany(x => x.ExternalLogins.Where(a => !a.IsDeleted && a.LoginType == ExternalLoginTypeEnum.Telegram)).DistinctBy(x => x.Value).ToListAsync(CancellationToken.None);

        foreach (var orgUser in botNotificationsAdmins)
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

                _handleUpdateService?.SendMessages(new Bot.Dtos.SendMsgToUserDto
                {
                    ChatId = chatId,
                    Message = sb.ToString()
                });
            }
        }
    }


    /// <summary>
    /// Отправка уведомаления о выиграном заказе
    /// </summary>
    /// <param name="offerId"></param>
    /// <returns></returns>
    private async Task SendChoosenOferToUsers(Guid offerId)
    {
        var tripId = await UnitOfWork.GetSet<DbTripRequestOffer>().Where(x => x.Id == offerId && !x.IsDeleted).Select(x => x.TripRequest.Identifiers.Select(a => a.Identifier).FirstOrDefault()).FirstOrDefaultAsync();

        var orgUsers = await UnitOfWork.GetSet<DbTripRequestOffer>().Where(x => x.Id == offerId && !x.IsDeleted).Select(x => x.Carrier).SelectMany(x => x.Accounts.Where(a => !a.Account.IsDeleted && a.AccountType == OrganisationAccountTypeEnum.Operator || a.AccountType == OrganisationAccountTypeEnum.Director).Select(a => a.Account))
            .SelectMany(x => x.ExternalLogins.Where(a => !a.IsDeleted && a.LoginType == ExternalLoginTypeEnum.Telegram)).ToListAsync();

        foreach (var orgUser in orgUsers)
        {
            if (long.TryParse(orgUser.Value, out long chatId))
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Ранее Вы откликались на заказ №{tripId}");
                sb.AppendLine($"Вы выбраны в качестве перевозчика на данный заказ");
                sb.AppendLine($"Для оформления документов и уточнения деталей свяжитесь с менеджером по телефонам +79002944275 или +79002783868");

                _handleUpdateService?.SendMessages(new Bot.Dtos.SendMsgToUserDto
                {
                    ChatId = chatId,
                    Message = sb.ToString()
                });
            }
        }
    }

    /// <summary>
    /// отправка уведомления о не выиграном заказе
    /// </summary>
    /// <param name="tripRequestId"></param>
    /// <returns></returns>
    private async Task SendUnChoosenOferToUsers(Guid tripRequestId)
    {
        var replays = await UnitOfWork.GetSet<DbTripRequestReplay>().Where(x => x.TripRequestId == tripRequestId && !x.IsDeleted && !x.Carrier.IsDeleted)
            .Select(x => x.Id).ToListAsync();

        var tripId = await UnitOfWork.GetSet<DbTripRequest>().Where(x => x.Id == tripRequestId).Select(x => x.Id).FirstAsync();

        var unchosenCarrier = UnitOfWork.GetSet<DbTripRequestOffer>().Where(x => x.TripRequestId == tripRequestId && !x.IsDeleted && !x.Chosen).Select(x => x.CarrierId);

        foreach (var replay in replays)
        {
            var orgUsers = await UnitOfWork.GetSet<DbTripRequestReplay>().Where(x => x.Id == replay).Select(x => x.Carrier).Where(x => unchosenCarrier.Any(a => a == x.Id)).SelectMany(x => x.Accounts.Where(a => !a.Account.IsDeleted && a.AccountType == OrganisationAccountTypeEnum.Operator || a.AccountType == OrganisationAccountTypeEnum.Director).Select(a => a.Account))
                .SelectMany(x => x.ExternalLogins.Where(a => !a.IsDeleted && a.LoginType == ExternalLoginTypeEnum.Telegram)).ToListAsync();

            foreach (var orgUser in orgUsers)
            {
                if (long.TryParse(orgUser.Value, out long chatId))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"Ранее Вы откликались на заказ №{tripId}");
                    sb.AppendLine($"К сожалению, Вас не выбрали в качестве перевозчика по данному заказу");
                    sb.AppendLine($"Ждем Ваших откликов на другие заказы");
                    sb.AppendLine($"Просмотреть список актуальных заказов Вы можете воспользовавшись командой /requests");

                    _handleUpdateService?.SendMessages(new Bot.Dtos.SendMsgToUserDto
                    {
                        ChatId = chatId,
                        Message = sb.ToString()
                    });

                }
            }
        }
    }

    private async Task SendChancelOferToUsers(Guid tripRequestId)
    {
        var replays = await UnitOfWork.GetSet<DbTripRequestReplay>().Where(x => x.TripRequestId == tripRequestId && !x.IsDeleted && !x.Carrier.IsDeleted)
            .Select(x => x.Id).ToListAsync();

        var tripId = await UnitOfWork.GetSet<DbTripRequest>().Where(x => x.Id == tripRequestId).Select(x => x.Id).FirstAsync();

        var offerdCarriers = UnitOfWork.GetSet<DbTripRequestOffer>().Where(x => x.TripRequestId == tripRequestId && !x.IsDeleted).Select(x => x.CarrierId);

        foreach (var replay in replays)
        {
            var orgUsers = await UnitOfWork.GetSet<DbTripRequestReplay>().Where(x => x.Id == replay).Select(x => x.Carrier).Where(x => offerdCarriers.Any(a => a == x.Id)).SelectMany(x => x.Accounts.Where(a => !a.Account.IsDeleted && a.AccountType == OrganisationAccountTypeEnum.Operator || a.AccountType == OrganisationAccountTypeEnum.Director).Select(a => a.Account))
                .SelectMany(x => x.ExternalLogins.Where(a => !a.IsDeleted && a.LoginType == ExternalLoginTypeEnum.Telegram)).ToListAsync();

            foreach (var orgUser in orgUsers)
            {
                if (long.TryParse(orgUser.Value, out long chatId))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"Ранее Вы откликались на заказ №{tripId}");
                    sb.AppendLine($"К сожалению, заказ был отменён");
                    sb.AppendLine($"Ждем Ваших откликов на другие заказы");
                    sb.AppendLine($"Просмотреть список актуальных заказов Вы можете воспользовавшись командой /requests");

                    _handleUpdateService?.SendMessages(new Bot.Dtos.SendMsgToUserDto
                    {
                        ChatId = chatId,
                        Message = sb.ToString()
                    });

                }
            }
        }
    }

    #endregion
}