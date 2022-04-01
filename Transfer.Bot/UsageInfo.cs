using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Transfer.Bot.Menu;
using Transfer.Common;

namespace Transfer.Bot
{
    public static class UsageInfo
    {
        public static async Task<Message> SendUsageInfo(this ITelegramBotClient bot, Message message, IUnitOfWork unitOfWork, ILogger Logger = null)
        {
            await unitOfWork.SetState(message.Chat.Id);
            
            var sb = new StringBuilder();
            sb.AppendLine("Доступные команды:");
            sb.AppendLine($"{Actions.RequestList.ActionName} - Список актуальных заказов");
            sb.AppendLine($"{Actions.SetActive.ActionActiveEnableName} - Включить уведомления");
            sb.AppendLine($"{Actions.SetActive.ActionActiveDisableName} - Временно отключить уведомления");

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: sb.ToString(),
                                                  replyMarkup: BaseMenu.GetBasicManuInline());
        }
    }
}
;