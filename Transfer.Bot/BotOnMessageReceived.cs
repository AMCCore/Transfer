using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Transfer.Bot.Actions;
using Transfer.Common;
using Transfer.Common.Extensions;
using Transfer.Dal.Entities;

namespace Transfer.Bot
{
    public static class BotOnMessageReceived
    {
        public static async Task OnMessageReceived(this ITelegramBotClient bot, Message message, IUnitOfWork unitOfWork, ILogger logger = null)
        {
            logger?.LogInformation("Receive message type: {messageType}", message.Type);
            if (message.Type != MessageType.Text )
            {
                await bot.SendTextMessageAsync(
                    chatId: message.From.Id,
                    text: "Данный тип сообщения не поддержиается. Я принимаю только текст.",
                    parseMode: ParseMode.Html,
                    replyMarkup: new ReplyKeyboardRemove());
                
                return;
            }

            var user = await unitOfWork.GetSet<DbAccount>().Where(x => x.ExternalLogins.Any(a => !a.IsDeleted && a.LoginType == Common.Enums.ExternalLoginEnum.Telegram && a.Value == message.From.Id.ToString())).FirstOrDefaultAsync();
            if(user == null)
            {
                //email
                var ruser = await unitOfWork.GetSet<DbAccount>().FirstOrDefaultAsync(x => x.Email == message.Text);
                if (ruser == null)
                {
                    //registration 1
                    await bot.SendTextMessageAsync(
                        chatId: message.From.Id,
                        text: "Для использования бота требуется зарегистрироваться. Направьте мне свой email под которым вы зарегестрированы на сайте.",
                        parseMode: ParseMode.Html,
                        replyMarkup: new ReplyKeyboardRemove());
                }
                else if(ruser.IsDeleted)
                {
                    //user is blocked
                    await bot.SendTextMessageAsync(
                        chatId: message.From.Id,
                        text: "Ваша учётная запись отключена. Обратитесь в службу поддержки.",
                        parseMode: ParseMode.Html,
                        replyMarkup: new ReplyKeyboardRemove());
                }
                else
                {
                    await unitOfWork.AddEntityAsync(new DbExternalLogin
                    {
                        AccountId = ruser.Id,
                        LoginType = Common.Enums.ExternalLoginEnum.Telegram,
                        Value = message.From.Id.ToString(),
                    });
                    await unitOfWork.SetState(ruser.Id, "Registered");
                    await bot.SendTextMessageAsync(
                        chatId: message.From.Id,
                        text: "Ваша учётная активирована. Бот к вашим услугам.",
                        parseMode: ParseMode.Html,
                        replyMarkup: new ReplyKeyboardRemove());
                }
            }
            else if(user.IsDeleted)
            {
                //user is blocked
                await bot.SendTextMessageAsync(
                    chatId: message.From.Id,
                    text: "Ваша учётная запись отключена. Обратитесь в службу поддержки.",
                    parseMode: ParseMode.Html,
                    replyMarkup: new ReplyKeyboardRemove());
            }
            else
            {
                var action = message.Text!.Split(' ')[0] switch
                {
                    SetActive.ActionActiveEnableName => bot.SetState(message, unitOfWork, true, logger),
                    SetActive.ActionActiveDisableName => bot.SetState(message, unitOfWork, false, logger),
                    RequestList.ActionName => bot.GetRequestList(message, unitOfWork, logger),
                    _ => bot.SendUsageInfo(message, unitOfWork, logger)
                };

                Message sentMessage = await action;
                logger?.LogInformation("The message was sent with id: {sentMessageId}", sentMessage.MessageId);
            }
        }
    }
}
