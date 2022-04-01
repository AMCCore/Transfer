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

    public async static Task<Message> GetRequestList(this ITelegramBotClient bot, Message message, IUnitOfWork unitOfWork, ILogger Logger = null)
    {
        var user = await unitOfWork.GetSet<DbAccount>().Where(x => x.ExternalLogins.Any(a => !a.IsDeleted && a.LoginType == Common.Enums.ExternalLoginEnum.Telegram && a.Value == message.Chat.Id.ToString())).Select(x => x.Id).FirstAsync();
        var requests = await unitOfWork.GetSet<DbTripRequest>().Where(x => !x.IsDeleted && x.State == Common.Enums.TripRequestStateEnum.Active && x.TripDate > DateTime.Now).OrderByDescending(x => x.DateCreated).Take(100).ToListAsync();
        if(!requests.Any())
        {
            return await bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Актуальных заказов не найдено",
                replyMarkup: BaseMenu.backtomenu);
        }
        foreach(var r in requests)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Заказ от компании {r.Charterer.Name}");
            sb.AppendLine($"Дата отправления: {r.TripDate:dd.MM.yyyy}");
            sb.AppendLine($"Место отправления: {r.AddressFrom}");
            sb.AppendLine($"Место прибытия: {r.AddressTo}");
            sb.AppendLine($"Кол-во пассажиров: {r.Passengers}");

            await bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: sb.ToString());
        }
        return await bot.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Это все актуальные заказы на данный момент",
            replyMarkup: BaseMenu.backtomenu);
    }
}
