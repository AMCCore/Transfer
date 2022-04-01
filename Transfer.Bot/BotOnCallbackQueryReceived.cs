using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Transfer.Bot
{
    public static class BotOnCallbackQueryReceived
    {
        public static async Task OnCallbackQueryReceived(this ITelegramBotClient bot, CallbackQuery callbackQuery, ILogger logger = null)
        {
            await bot.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            await bot.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received {callbackQuery.Data}");
        }
    }
}
