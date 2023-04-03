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
using Transfer.Common.Security;
using Transfer.Common.Settings;
using Transfer.Dal.Entities;
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
    private readonly ITripRequestSecurityService _securityService;

#if !DEBUG

    public TripRequestController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<TripRequestController> logger, IMapper mapper, HandleUpdateService handleUpdateService, ITripRequestSecurityService securityService) : base(transferSettings, unitOfWork, logger, mapper)
    {
        _handleUpdateService = handleUpdateService;
        _securityService = securityService;
    }

#else

    public TripRequestController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<TripRequestController> logger, IMapper mapper, ITripRequestSecurityService securityService) : base(transferSettings, unitOfWork, logger, mapper)
    {
        _securityService = securityService;
    }

#endif

    private async Task<RequestSearchFilter> GetDataFromDb(RequestSearchFilter filter = null)
    {
        //await UnitOfWork.TripRequestStateRegulate();

        filter ??= new RequestSearchFilter(new List<TripRequestSearchResultItem>(), TransferSettings.TablePageSize);
        var query = UnitOfWork.GetSet<DbTripRequest>().Where(x => !x.IsDeleted).AsQueryable();

        if (!_securityService.IsAdmin)
        {
            if (filter.MyRequests)
            {
                var orgs = _securityService.HasOrganisationsForRight(TripRequestRights.ViewList).Select(x => (Guid?)x).ToArray();
                query = query.Where(x => orgs.Contains(x.ChartererId));
            }
            else if (!_securityService.HasRightForSomeOrganisation(TripRequestRights.ViewList))
            {
                query = query.Where(x => x.Id == Guid.Empty);
            }

        }

        //статусы
        if (filter.State == (int)TripRequestSearchStateEnum.StateNew)
        {
            var sts = new[] { TripRequestStateEnum.Active.GetEnumGuid(), TripRequestStateEnum.New.GetEnumGuid() };
            query = query.Where(x => sts.Contains(x.State));
        }
        else if (filter.State == (int)TripRequestSearchStateEnum.StateComplete)
        {
            var sts = new[] { TripRequestStateEnum.Completed.GetEnumGuid(), TripRequestStateEnum.CompletedNoConfirm.GetEnumGuid(), TripRequestStateEnum.Done.GetEnumGuid() };
            query = query.Where(x => sts.Contains(x.State));
        }
        else if (filter.State == (int)TripRequestSearchStateEnum.StateOnair)
        {
            query = query.Where(x => x.State == TripRequestStateEnum.CarrierSelected.GetEnumGuid());
        }
        else if (filter.State == (int)TripRequestSearchStateEnum.StateCanceled)
        {
            var sts = new[] { TripRequestStateEnum.Canceled.GetEnumGuid(), TripRequestStateEnum.Overdue.GetEnumGuid() };
            query = query.Where(x => sts.Contains(x.State));
        }
        else if (filter.State == (int)TripRequestSearchStateEnum.StateArchived)
        {
            query = query.Where(x => x.State == TripRequestStateEnum.Archived.GetEnumGuid());
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
        if (!_securityService.HasRightForSomeOrganisation(TripRequestRights.ViewList))
            return Unauthorized();

        var result = await GetDataFromDb();
        return View(result);
    }

    [HttpPost]
    [Route("TripRequests")]
    public async Task<IActionResult> Search(RequestSearchFilter filter)
    {
        if (!_securityService.HasRightForSomeOrganisation(TripRequestRights.ViewList))
            return Unauthorized();

        var result = await GetDataFromDb(filter);

        return PartialView("SearchResults", result);
    }

    [Route("TripRequests/My")]
    [HttpGet]
    public async Task<IActionResult> SearchMyTripRequests()
    {
        if (!_securityService.HasRightForSomeOrganisation(TripRequestRights.ViewList))
            return Unauthorized();

        var result = await GetDataFromDb(new RequestSearchFilter { MyRequests = true });
        return View("Search", result);
    }

    [HttpGet]
    [Route("TripRequest/{requestId}")]
    public async Task<IActionResult> TripRequestShow(Guid requestId, CancellationToken token = default)
    {
        if (!_securityService.HasRightForSomeOrganisation(TripRequestRights.ViewList))
            return Unauthorized();

        var entity = await UnitOfWork.GetSet<DbTripRequest>().FirstOrDefaultAsync(ss => ss.Id == requestId, token);
        if (entity == null)
            return NotFound();

        if (!_securityService.HasRightForSomeOrganisation(TripRequestRights.ViewList))
            return Unauthorized();

        var model = Mapper.Map<TripRequestWithOffersDto>(entity);
        await SetNextStates(model, StateMachineEnum.TripRequest, entity.ChartererId, token);

        model.Offers = await UnitOfWork.GetSet<DbTripRequestOffer>().Where(x => !x.IsDeleted && x.TripRequestId == requestId).Include(x => x.Carrier).Select(x => Mapper.Map<TripRequestOfferSearchResultItem>(x)).ToListAsync(token);
            
        return View("Show", model);
    }

    [HttpGet]
    [Route("TripRequest/Edit/{requestId}")]
    public async Task<IActionResult> TripRequestEdit(Guid requestId, CancellationToken token = default)
    {
        return BadRequest();

        var entity = await UnitOfWork.GetSet<DbTripRequest>().FirstOrDefaultAsync(ss => ss.Id == requestId, token);
        if (entity == null)
            return NotFound();

        var model = Mapper.Map<TripRequestDto>(entity);
        await SetNextStates(model, StateMachineEnum.TripRequest, token: token);
        return View("Save", model);
    }

    [HttpGet]
    [Route("TripRequest/New")]
    public IActionResult NewTripRequest()
    {
        if (!_securityService.HasRightForSomeOrganisation(TripRequestRights.Create))
            return Unauthorized();

        return View("Save", new TripRequestDto
        {
            TripDate = DateTime.Now.AddDays(1).ChangeTime(9, 0),
            PaymentType = (int)PaymentTypeEnum.Checking,
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("TripRequest/Save")]
    public async Task<IActionResult> Save([FromForm] TripRequestDto model, CancellationToken token = default)
    {
        if (!_securityService.HasRightForSomeOrganisation(TripRequestRights.Create))
            return Unauthorized();


        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Одно или несколько полей не заполнены";
            return View("Save", model);
        }

        if (model.Id.IsNullOrEmpty())
        {
            await UnitOfWork.BeginTransactionAsync(token);

            model.Id = Guid.NewGuid();
            var entity = Mapper.Map<DbTripRequest>(model);

            entity.StateEnum = TripRequestStateEnum.Active;

            if (string.IsNullOrWhiteSpace(entity.ContactFio))
            {
                entity.ContactFio = entity.СhartererName;
            }

            //Временное решение!!! не делать так 
            entity.ChartererId = await UnitOfWork.GetSet<DbAccount>().Where(x => x.Id == _securityService.CurrentAccountId).SelectMany(x => x.Organisations).Select(x => x.OrganisationId).FirstOrDefaultAsync(token);

            entity.RegionFromId = (await GetOrCreateRegion(model.RegionFromName, token))?.Id;
            entity.RegionToId = (await GetOrCreateRegion(model.RegionToName, token))?.Id;

            entity = await UnitOfWork.AddEntityAsync(entity, token: token);
            await UnitOfWork.AddEntityAsync(new DbTripRequestIdentifier { TripRequestId = model.Id }, token: token);
            await SetTripOptions(entity, model, token);


            var appropriateOrgIdsq = UnitOfWork.GetSet<DbOrganisation>().AsQueryable();
            var regs = new List<Guid>();
            if (entity.RegionFromId.HasValue)
            {
                regs.Add(entity.RegionFromId.Value);
            }
            if (entity.RegionToId.HasValue)
            {
                regs.Add(entity.RegionToId.Value);
            }

            if (regs.Any())
            {
                appropriateOrgIdsq = appropriateOrgIdsq.Where(x => x.WorkingArea.Any(wa => regs.Contains(wa.RegionId)));
            }

            var appropriateOrgIds = await appropriateOrgIdsq.Select(x => x.Id).ToListAsync(token);

            foreach (var orgId in appropriateOrgIds)
            {
                await UnitOfWork.AddEntityAsync(new DbTripRequestReplay
                {
                    TripRequestId = entity.Id,
                    CarrierId = orgId,
                }, token: token);
            }

            await UnitOfWork.AddToHistoryLog(entity, "Создание запроса на перевозку", token: token);
            await UnitOfWork.CommitAsync(token);

            await SendReplaysToUsers(entity.Id, token);
            await SendReplaysToWatchers(entity.Id, token);

            return RedirectToAction(nameof(TripRequestShow), new { requestId = model.Id });
        }

        return BadRequest();
    }

    [HttpGet]
    [Route("TripRequest/StateChange")]
    public async Task<IActionResult> TripRequestStateChange(Guid requestId, Guid stateId, CancellationToken token = default)
    {
        var entity = await UnitOfWork.GetSet<DbTripRequest>().Where(ss => ss.Id == requestId).FirstOrDefaultAsync(token)
            ?? throw new ArgumentNullException(nameof(requestId));

        var fromStateToState = await UnitOfWork.GetSet<DbStateMachineFromStatus>()
            .Where(x => x.StateMachine == StateMachineEnum.TripRequest && x.FromStateId == entity.State && !x.StateMachineAction.IsSystemAction && x.StateMachineAction.ToStateId == stateId && x.StateMachineAction.StateMachine == StateMachineEnum.TripRequest)
            .Include(x => x.StateMachineAction).ThenInclude(x => x.ToState)
            .FirstOrDefaultAsync(token)
            ?? throw new ArgumentNullException(nameof(requestId));

        if (_securityService.HasRightForSomeOrganisation(fromStateToState.RightCode ?? Guid.Empty))
        {
            entity.State = fromStateToState.StateMachineAction.ToState.Id;
            await UnitOfWork.SaveChangesAsync(token);
            await UnitOfWork.AddToHistoryLog(entity, "Статус запроса на перевозку изменён", $"Новый статус: {fromStateToState.StateMachineAction.ToState.Name}", token);

            return RedirectToAction(nameof(TripRequestShow), new { requestId });
        }

        return Unauthorized();
    }

    private async Task SetTripOptions(DbTripRequest entity, TripRequestDto model, CancellationToken token = default)
    {
        foreach (var to in await UnitOfWork.GetSet<DbTripRequestOption>().Where(x => x.TripRequestId == entity.Id).ToListAsync(token))
        {
            await UnitOfWork.DeleteAsync(to, token);
        }
        if (model.ChildTrip)
        {
            await UnitOfWork.AddEntityAsync(new DbTripRequestOption
            {
                Id = Guid.NewGuid(),
                TripRequestId = entity.Id,
                TripOptionId = TripOptionsEnum.ChildTrip.GetEnumGuid()
            }, token: token);
        }
        if (model.StandTrip)
        {
            await UnitOfWork.AddEntityAsync(new DbTripRequestOption
            {
                Id = Guid.NewGuid(),
                TripRequestId = entity.Id,
                TripOptionId = TripOptionsEnum.IdleTrip.GetEnumGuid()
            }, token: token);
        }
        if (model.PaymentType == (int)PaymentTypeEnum.Card)
        {
            await UnitOfWork.AddEntityAsync(new DbTripRequestOption
            {
                Id = Guid.NewGuid(),
                TripRequestId = entity.Id,
                TripOptionId = TripOptionsEnum.CardPayment.GetEnumGuid()
            }, token: token);

        }
        else if (model.PaymentType == (int)PaymentTypeEnum.Cash)
        {
            await UnitOfWork.AddEntityAsync(new DbTripRequestOption
            {
                Id = Guid.NewGuid(),
                TripRequestId = entity.Id,
                TripOptionId = TripOptionsEnum.CashPayment.GetEnumGuid()
            }, token: token);
        }
    }

    private async Task<DbRegion> GetOrCreateRegion(string regionName, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(regionName))
            return null;

        var reg = await UnitOfWork.GetSet<DbRegion>().FirstOrDefaultAsync(ss => ss.Name.ToLower().Contains(regionName.ToLower()) || ss.RegionAlias.Any(x => x.Name.ToLower().Contains(regionName.ToLower())), token);
        if (reg == null)
        {
            await UnitOfWork.AddToHistoryLog(SystemEventEnum.UnknownRegionName.GetEnumGuid(), "Из ФИАСа пришел нераспознанный регион", $"{regionName}", token);
            //reg = new DbRegion
            //{
            //    Name = regionName
            //};
            //await UnitOfWork.AddEntityAsync(reg, CancellationToken.None);
        }
        return reg;
    }

    [HttpGet]
    [Route("TripRequest/{requestId}/CarrierChoose/{offerId}")]
    public async Task<IActionResult> TripRequestSetCarrierChoosen(Guid requestId, Guid offerId, CancellationToken token = default)
    {
        var entity = await UnitOfWork.GetSet<DbTripRequestOffer>().Where(ss => ss.Id == offerId && !ss.IsDeleted && ss.TripRequestId == requestId).FirstOrDefaultAsync(token) ?? throw new ArgumentNullException(nameof(offerId));
        var trip = entity.TripRequest;
        if (trip.StateEnum != TripRequestStateEnum.Active)
            return BadRequest();


        if (_securityService.HasRightForSomeOrganisation(TripRequestRights.CarrierChoose, trip.ChartererId))
        {
            entity.Chosen = true;
            trip.StateEnum = TripRequestStateEnum.CarrierSelected;
            await UnitOfWork.SaveChangesAsync(token);
            await UnitOfWork.AddToHistoryLog(trip, "Перевозчик выбран", $"Перевозчик: {entity.Carrier.Name}({entity.Carrier.Id}), сумма предложения: {entity.Amount}", token);
            await UnitOfWork.AddToHistoryLog(trip, "Статус запроса на перевозку изменён", $"Новый статус: {TripRequestStateEnum.CarrierSelected.GetEnumDescription()}", token);

            await SendChoosenOferToUsers(entity.Id);
            await SendUnChoosenOferToUsers(trip.Id);

            return RedirectToAction(nameof(TripRequestShow), new { requestId = entity.TripRequestId });
        }

        return Unauthorized();
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
    public async Task<IActionResult> MakeOffer(Guid requestId, CancellationToken token = default)
    {
        var replay = await UnitOfWork.GetSet<DbTripRequestReplay>().Include(x => x.TripRequest).FirstOrDefaultAsync(x => x.Id == requestId, token);
        if (replay == null || replay.IsDeleted || replay.TripRequest.State == TripRequestStateEnum.Archived.GetEnumGuid())
        {
            TempData[errMsgName] = "Поездка не найдена";
            return RedirectToAction(nameof(MakeOfferError));
        }
        if (replay.TripRequest.TripDate <= DateTime.Now || replay.TripRequest.State == TripRequestStateEnum.Completed.GetEnumGuid())
        {
            TempData[errMsgName] = "Поездка уже прошла";
            return RedirectToAction(nameof(MakeOfferError));
        }
        if (replay.TripRequest.State != TripRequestStateEnum.Active.GetEnumGuid())
        {
            TempData[errMsgName] = "Исполнитель на данную поездку уже выбран";
            return RedirectToAction(nameof(MakeOfferError));
        }
        if (await UnitOfWork.GetSet<DbTripRequestOffer>().AnyAsync(x => x.TripRequestId == replay.TripRequestId && x.CarrierId == replay.CarrierId, token))
        {
            TempData[errMsgName] = "Ваше предложение уже учтено";
            return RedirectToAction(nameof(MakeOfferError));
        }

        var model = Mapper.Map<TripRequestOfferDto>(replay.TripRequest);
        await SetNextStates(model, StateMachineEnum.TripRequest, replay.TripRequest.ChartererId, token);
        model.CarrierId = replay.CarrierId;
        return View("MakeOffer", model);
    }

    [HttpGet]
    [Route("MakeRequestOffer/{requestId}")]
    public async Task<IActionResult> MakeRequestOffer(Guid requestId, CancellationToken token = default)
    {
        if (!_securityService.HasRightForSomeOrganisation(TripRequestRights.MakeOffer))
            return Unauthorized();

        var orgs = _securityService.HasOrganisationsForRight(TripRequestRights.MakeOffer);

        if (orgs.Length != 1 && orgs[0] != Guid.Empty)
            return BadRequest();

        var org = orgs[0];

        var trip = await UnitOfWork.GetSet<DbTripRequest>().FirstOrDefaultAsync(x => x.Id == requestId, token);

        if (trip == null)
            return NotFound();

        if (trip.TripDate <= DateTime.Now || trip.State != TripRequestStateEnum.Active.GetEnumGuid())
            return RedirectToHome();

        if (await UnitOfWork.GetSet<DbTripRequestOffer>().AnyAsync(x => x.TripRequestId == trip.Id && x.CarrierId == org, token))
            return RedirectToHome();

        var model = Mapper.Map<TripRequestOfferDto>(trip);
        await SetNextStates(model, StateMachineEnum.TripRequest, trip.ChartererId, token);
        model.CarrierId = org;
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
    private async Task SendReplaysToUsers(Guid tripRequestId, CancellationToken token = default)
    {
        var replays = await UnitOfWork.GetSet<DbTripRequestReplay>().Where(x => x.TripRequestId == tripRequestId && !x.IsDeleted && !x.Carrier.IsDeleted)
            .Select(x => x.Id).ToListAsync(token);

        var trip = await UnitOfWork.GetSet<DbTripRequest>().FirstAsync(x => x.Id == tripRequestId, token);

        foreach (var replay in replays)
        {
            var orgUsers = await UnitOfWork.GetSet<DbTripRequestReplay>().Where(x => x.Id == replay).Select(x => x.Carrier).SelectMany(x => x.Accounts.Where(a => !a.Account.IsDeleted && a.AccountType == OrganisationAccountTypeEnum.Operator || a.AccountType == OrganisationAccountTypeEnum.Director).Select(a => a.Account))
                .SelectMany(x => x.ExternalLogins.Where(a => !a.IsDeleted && a.LoginType == ExternalLoginTypeEnum.Telegram)).ToListAsync(token);

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
    private async Task SendReplaysToWatchers(Guid tripRequestId, CancellationToken token = default)
    {
        var trip = await UnitOfWork.GetSet<DbTripRequest>().FirstAsync(x => x.Id == tripRequestId, token);
        var ttd = AdminAccessRights.BotNotifications.GetEnumGuid();

        var q2 = UnitOfWork.GetSet<DbAccount>().Where(x => !x.IsDeleted && x.AccountRights.Any(y => y.RightId == ttd)).Select(x => x.Id).AsQueryable();

        var botNotificationsAdmins = await UnitOfWork.GetSet<DbExternalLogin>().Where(x => !x.IsDeleted && q2.Contains(x.AccountId) && x.LoginType == ExternalLoginTypeEnum.Telegram).ToListAsync(token);

        foreach (var orgUser in botNotificationsAdmins.DistinctBy(x => x.Value).ToList())
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

    protected override IQueryable<DbStateMachineAction> GetNextStatesFromDB(StateMachineDto model, StateMachineEnum stateMachine, Guid? organisationId = null)
    {
        var q = UnitOfWork.GetSet<DbStateMachineAction>()
            .Where(x => !x.IsSystemAction && x.StateMachine == stateMachine &&
            x.FromStates.Any(y => y.StateMachine == stateMachine && y.FromStateId == model.State)).Include(x => x.FromStates).OrderBy(x => x.SortingOrder).AsQueryable();

        return q;
    }

    public override async Task SetNextStates(StateMachineDto model, StateMachineEnum stateMachine, Guid? organisationId = null, CancellationToken token = default)
    {
        var ns = await GetNextStatesFromDB(model, stateMachine, organisationId).ToListAsync(token);
        var resp = new List<NextStateDto>();

        foreach (var s in ns)
        {
            var fr = s.FromStates.FirstOrDefault(x => x.FromStateId == model.State);
            if (fr == null)
                continue;

            if (fr.RightCode.IsNullOrEmpty() && !_securityService.IsAdmin)
                continue;

            if (!fr.RightCode.IsNullOrEmpty() && !_securityService.HasRightForSomeOrganisation((Guid)fr.RightCode, organisationId))
                continue;

            resp.Add(new NextStateDto
            {
                NextStateId = s.ToStateId,
                ButtonName = s.ActionName,
                ConfirmText = (!string.IsNullOrWhiteSpace(s.ConfirmText) ? s.ConfirmText : null)
            });
        }

        model.NextStates = resp;
    }
}