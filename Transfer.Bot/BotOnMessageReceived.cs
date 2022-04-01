using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Transfer.Common;

namespace Transfer.Bot
{
    public static class BotOnMessageReceived
    {
        public static async Task OnMessageReceived(this ITelegramBotClient bot, Message message, IUnitOfWork unitOfWork, ILogger logger = null)
        {
            logger?.LogInformation("Receive message type: {messageType}", message.Type);
            if (message.Type != MessageType.Text)
                return;

            var action = message.Text!.Split(' ')[0] switch
            {
                _ => bot.SendUsageInfo(message, logger)
            };

            Message sentMessage = await action;
            logger?.LogInformation("The message was sent with id: {sentMessageId}", sentMessage.MessageId);
        }
    }
}
