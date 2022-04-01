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
using Transfer.Common;
using Transfer.Dal.Entities;

namespace Transfer.Bot.Actions;

internal static class RequestList
{
    public const string ActionName = "/requests";

    public async static Task GetRequestList(this ITelegramBotClient bot, Message message, IUnitOfWork unitOfWork, ILogger Logger = null)
    {
        var user = await unitOfWork.GetSet<DbAccount>().Where(x => x.ExternalLogins.Any(a => !a.IsDeleted && a.LoginType == Common.Enums.ExternalLoginEnum.Telegram && a.Value == message.Chat.Id.ToString())).Select(x => x.Id).FirstAsync();
        var requests = await unitOfWork.GetSet<DbTripRequest>().Where(x => !x.IsDeleted && x.State == Common.Enums.TripRequestStateEnum.Active).Take(100).ToListAsync();
        if(!requests.Any())
        {

        }
        foreach(var r in requests)
        {

        }
    }
}
