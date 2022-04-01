using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Transfer.Bot.Actions;
using Transfer.Bot.Menu;
using Transfer.Common;

namespace Transfer.Bot
{
    public static class BotOnCallbackQueryReceived
    {
        public static async Task OnCallbackQueryReceived(this ITelegramBotClient bot, CallbackQuery callbackQuery, IUnitOfWork unitOfWork, ILogger logger = null)
        {
            string message = callbackQuery.Data;

            await bot.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id, null);

            var action = callbackQuery.Data!.Split(' ')[0] switch
            {
                RequestList.ActionInlineName => bot.GetRequestList(callbackQuery.Message.Chat.Id, unitOfWork, logger),
                BaseMenu.ActionInlineName => bot.GetMainMenuWithInlineMenu(callbackQuery.Message.Chat.Id, unitOfWork, logger),
                _ => bot.SendUsageInfo(callbackQuery.Message.Chat.Id, unitOfWork, logger)
            };

            await action;
        }


    }
}
