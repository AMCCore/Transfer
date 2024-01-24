using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Bl.Dto.TripRequest;
using Transfer.Common;
using Transfer.Common.Enums.AccessRights;
using Transfer.Common.Enums.States;
using Transfer.Common.Extensions;
using Transfer.Common.Security;
using Transfer.Common.Settings;
using Transfer.Dal.Entities;

namespace Transfer.Web.Controllers;

[Authorize]
[Route("[controller]")]
public class MakeOfferController : BaseStateController
{
    const string errMsgName = "errMsg";

    private readonly ITripRequestSecurityService _securityService;

    public MakeOfferController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<TripRequestController> logger, IMapper mapper, ITripRequestSecurityService securityService) : base(transferSettings, unitOfWork, logger, mapper)
    {
        _securityService = securityService;
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
        if (replay == null || replay.TripRequest.State == TripRequestStateEnum.Archived.GetEnumGuid())
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
            TempData[errMsgName] = "Поездка завершена";
            return RedirectToAction(nameof(MakeOfferError));
        }
        if (await UnitOfWork.GetSet<DbTripRequestOffer>().AnyAsync(x => x.TripRequestId == replay.TripRequestId && x.CarrierId == replay.CarrierId, token))
        {
            TempData[errMsgName] = "Ваше предложение уже учтено";
            return RedirectToAction(nameof(MakeOfferError));
        }

        var model = Mapper.Map<TripRequestOfferDto>(replay.TripRequest);

        //await SetNextStates(model, replay.TripRequest.OrgCreatorId.Value, replay.TripRequest.ChartererId, replay.TripRequest.TripRequestOffers.Where(x => x.Chosen).Select(x => x.CarrierId).FirstOrDefault(), token);

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

        //await SetNextStates(input, token);

        model.CarrierId = org;
        return View("MakeOffer", model);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("MakeOffer/Save")]
    [AllowAnonymous]
    public async Task<IActionResult> OfferSave([FromForm] TripRequestOfferDto model, CancellationToken token = default)
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

        var req = await UnitOfWork.GetSet<DbTripRequest>().FirstOrDefaultAsync(x => x.Id == model.Id, token);

        var offer = new DbTripRequestOffer
        {
            CarrierId = model.CarrierId,
            Amount = model.Amount,
            Comment = model.Comment,
            TripRequestId = model.Id
        };

        await UnitOfWork.AddEntityAsync(offer, true, token);

        return RedirectToAction(nameof(MakeOfferSuccess));
    }
}
