using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Transfer.Bot
{
    public static class UsageInfo
    {
        public static async Task<Message> SendUsageInfo(this ITelegramBotClient bot, Message message, ILogger Logger = null)
        {
            const string usage = "Usage:\n" +
                     "/inline   - send inline keyboard\n" +
                     "/keyboard - send custom keyboard\n" +
                     "/remove   - remove custom keyboard\n" +
                     "/photo    - send a photo\n" +
                     "/request  - request location or contact";

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: usage,
                                                  replyMarkup: new ReplyKeyboardRemove());
        }
    }
}
