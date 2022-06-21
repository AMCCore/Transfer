using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Transfer.Bot.Dtos;
using Transfer.Bot.Menu;
using Transfer.Common;
using Transfer.Dal.Entities;

namespace Transfer.Bot.Actions;

internal static class RequestList
{
    public const string ActionName = "/requests";
    public const string ActionInlineName = "Requests";

    public async static Task<Message> GetRequestList(this ITelegramBotClient bot, long Sender, IUnitOfWork unitOfWork, ILogger Logger = null)
    {
        //var user = await unitOfWork.GetSet<DbAccount>().Where(x => x.ExternalLogins.Any(a => !a.IsDeleted && a.LoginType == Common.Enums.ExternalLoginEnum.Telegram && a.Value == message.Chat.Id.ToString())).Select(x => x.Id).FirstAsync();
        var qRequests = unitOfWork.GetSet<DbTripRequest>().Where(x => !x.IsDeleted && x.State == Common.Enums.TripRequestStateEnum.Active && x.TripDate > DateTime.Now).OrderByDescending(x => x.DateCreated).Take(100);
        var qMyOrgs = unitOfWork.GetSet<DbExternalLogin>()
            .Where(x => x.IsDeleted && x.LoginType == Common.Enums.ExternalLoginEnum.Telegram && x.Value == Sender.ToString())
            .SelectMany(x => x.Account.Organisations).Where(x => !x.Organisation.IsDeleted).Select(x => x.Organisation);

        //var regionIds = await qMyOrgs.SelectMany(x => x.WorkingArea).Select(x => x.RegionId).ToListAsync();

        var myOrgIds = await qMyOrgs.Select(x => x.Id).ToListAsync();

        var requests = await qRequests.Include(x => x.TripRequestOffers).Include(x => x.TripRequestReplays).ToListAsync();
        if(!requests.Any())
        {
            return await bot.SendTextMessageAsync(
                chatId: Sender,
                text: "Актуальных заказов не найдено",
                replyMarkup: BaseMenu.backtomenu);
        }
        foreach(var r in requests)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Заказ от: {r.Charterer?.Name ?? r.ContactFio}");
            sb.AppendLine($"Дата отправления: {r.TripDate:dd.MM.yyyy}");
            sb.AppendLine($"Место отправления: {r.AddressFrom}");
            sb.AppendLine($"Место прибытия: {r.AddressTo}");
            sb.AppendLine($"Кол-во пассажиров: {r.Passengers}");

            var rep = r.TripRequestReplays.FirstOrDefault(x => myOrgIds.Any(y => y == x.CarrierId));
            if (rep != null && !r.TripRequestOffers.Any(x => myOrgIds.Any(y => y == x.CarrierId)))
            {
                sb.AppendLine();
                sb.AppendLine($"Чтобы откликнуться перейдите по ссылке\nhttps://nexttripto.ru/MakeOffer/{rep.Id}");
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
