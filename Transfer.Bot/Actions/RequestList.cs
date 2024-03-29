﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Transfer.Bot.Dtos;
using Transfer.Bot.Menu;
using Transfer.Common;
using Transfer.Common.Enums.States;
using Transfer.Common.Extensions;
using Transfer.Dal;
using Transfer.Dal.Entities;

namespace Transfer.Bot.Actions;

public static class RequestList
{
    public const string ActionName = "/requests";
    public const string ActionInlineName = "Requests";

    public static async Task TestQuerys(long Sender, IUnitOfWork unitOfWork, CancellationToken token = default)
    {
        var qMyOrgs = unitOfWork.Query<DbExternalLogin>()
            .Where(x => x.LoginType == Common.Enums.ExternalLoginTypeEnum.Telegram && x.Value == Sender.ToString())
            .SelectMany(x => x.Account.Organisations).Select(x => x.Organisation);

        var myOrgIds = await qMyOrgs.Select(x => x.Id).ToListAsync();
        
        IQueryable<Guid?> myOrgRegionsIds;

        if (myOrgIds.Any())
        {
            myOrgRegionsIds = qMyOrgs.SelectMany(x => x.WorkingArea).Select(x => (Guid?)x.RegionId).Distinct();
        }
        else
        {
            myOrgRegionsIds = unitOfWork.Query<DbRegion>().Select(x => (Guid?)x.Id).Distinct();
        }



        //var user = await unitOfWork.GetSet<DbAccount>().Where(x => x.ExternalLogins.Any(a => !a.IsDeleted && a.LoginType == Common.Enums.ExternalLoginEnum.Telegram && a.Value == message.Chat.Id.ToString())).Select(x => x.Id).FirstAsync();
        var qRequests = unitOfWork.GetSet<DbTripRequest>().Where(x => x.State == TripRequestStateEnum.Active.GetEnumGuid() && x.TripDate > DateTime.Now)
            .Where(x => myOrgRegionsIds.Contains(x.RegionFromId) || myOrgRegionsIds.Contains(x.RegionToId))
            .OrderByDescending(x => x.DateCreated).Take(100);

        //var regionIds = await qMyOrgs.SelectMany(x => x.WorkingArea).Select(x => x.RegionId).ToListAsync();


        var requests = qRequests.Include(x => x.Identifiers).Include(x => x.TripRequestOffers).Include(x => x.TripRequestReplays);

        var r = await requests.ToListAsync(token);
    }

    public async static Task<Message> GetRequestList(this ITelegramBotClient bot, long Sender, IUnitOfWork unitOfWork, ILogger Logger = null, CancellationToken token = default)
    {
        //мои организации
        var qMyOrgs = unitOfWork.Query<DbExternalLogin>()
            .Where(x => x.LoginType == Common.Enums.ExternalLoginTypeEnum.Telegram && x.Value == Sender.ToString())
            .SelectMany(x => x.Account.Organisations).Select(x => x.Organisation);

        var myOrgIds = await qMyOrgs.Select(x => x.Id).ToListAsync(token);

        IQueryable<Guid?> myOrgRegionsIds;

        if(myOrgIds.Any())
        {
            myOrgRegionsIds = qMyOrgs.SelectMany(x => x.WorkingArea).Select(x => (Guid?)x.RegionId).Distinct();
        }
        else
        {
            myOrgRegionsIds = unitOfWork.Query<DbRegion>().Select(x => (Guid?)x.Id).Distinct();
        }

        //var user = await unitOfWork.GetSet<DbAccount>().Where(x => x.ExternalLogins.Any(a => !a.IsDeleted && a.LoginType == Common.Enums.ExternalLoginEnum.Telegram && a.Value == message.Chat.Id.ToString())).Select(x => x.Id).FirstAsync();
        var qRequests = unitOfWork.GetSet<DbTripRequest>().Where(x => x.State == TripRequestStateEnum.Active.GetEnumGuid() && x.TripDate > DateTime.Now)
            .Where(x => myOrgRegionsIds.Contains(x.RegionFromId) || myOrgRegionsIds.Contains(x.RegionToId))
            .OrderByDescending(x => x.DateCreated).Take(100);

        //var regionIds = await qMyOrgs.SelectMany(x => x.WorkingArea).Select(x => x.RegionId).ToListAsync();


        var requests = qRequests.Include(x => x.Identifiers).Include(x => x.TripRequestOffers).Include(x => x.TripRequestReplays);
        if(!await requests.AnyAsync(token))
        {
            return await bot.SendTextMessageAsync(
                chatId: Sender,
                text: "Актуальных заказов не найдено",
                replyMarkup: BaseMenu.backtomenu);
        }
        foreach(var r in await requests.ToListAsync(token))
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Заказ №{r.Identifiers.Select(x => x.Identifier).FirstOrDefault()}");
            sb.AppendLine($"Заказчик: {(!r.ChartererId.IsNullOrEmpty() ? r.Charterer.Name : r.СhartererName)}");
            sb.AppendLine($"Дата отправления: {r.TripDate:dd.MM.yyyy HH:mm}");
            sb.AppendLine($"Место отправления: {r.AddressFrom}");
            sb.AppendLine($"Место прибытия: {r.AddressTo}");
            sb.AppendLine($"Кол-во пассажиров: {r.Passengers}");
            
            //sb.AppendLine($"Заказ от: {r.Charterer?.Name ?? r.ContactFio}");
            //sb.AppendLine($"Дата отправления: {r.TripDate:dd.MM.yyyy}");
            //sb.AppendLine($"Место отправления: {r.AddressFrom}");
            //sb.AppendLine($"Место прибытия: {r.AddressTo}");
            //sb.AppendLine($"Кол-во пассажиров: {r.Passengers}");

            var rep = r.TripRequestReplays.FirstOrDefault(x => myOrgIds.Any(y => y == x.CarrierId));
            if (rep != null && !r.TripRequestOffers.Any(x => myOrgIds.Any(y => y == x.CarrierId)))
            {
                sb.AppendLine();
                sb.AppendLine("Чтобы откликнуться перейдите по ссылке");
                sb.AppendLine($"https://nexttripto.ru/MakeOffer/{rep.Id}");

                //sb.AppendLine();
                //sb.AppendLine($"Чтобы откликнуться перейдите по ссылке\nhttps://nexttripto.ru/MakeOffer/{rep.Id}");
            }

            await bot.SendTextMessageAsync(
                chatId: Sender,
                text: sb.ToString());
        }
        return await bot.SendTextMessageAsync(
            chatId: Sender,
            text: "Это все актуальные заказы на данный момент",
            replyMarkup: BaseMenu.backtomenu);
    }
}
