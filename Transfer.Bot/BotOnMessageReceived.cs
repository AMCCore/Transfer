using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Transfer.Bot.Actions;
using Transfer.Bot.Menu;
using Transfer.Common;
using Transfer.Common.Enums.AccessRights;
using Transfer.Common.Extensions;
using Transfer.Dal.Entities;

namespace Transfer.Bot
{
    public static class BotOnMessageReceived
    {
        public static async Task OnMessageReceived(this ITelegramBotClient bot, Message message, IUnitOfWork unitOfWork, ILogger logger = null, IMailModule mailModule = null)
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

            var user = await unitOfWork.GetSet<DbAccount>().Where(x => x.ExternalLogins.Any(a => !a.IsDeleted && a.LoginType == Common.Enums.ExternalLoginTypeEnum.Telegram && a.Value == message.From.Id.ToString())).FirstOrDefaultAsync();
            if(user == null)
            {
                //email
                var ruser = await unitOfWork.GetSet<DbAccount>().Include(x => x.AccountRights).FirstOrDefaultAsync(x => x.Email == message.Text);
                //registration 1
                if (ruser == null)
                {
                    await bot.SendTextMessageAsync(
                        chatId: message.From.Id,
                        text: "Для использования бота требуется зарегистрироваться. Направьте мне свой email под которым вы зарегестрированы на сайте.",
                        parseMode: ParseMode.Html,
                        replyMarkup: new ReplyKeyboardRemove());
                }
                //user is blocked
                else if (ruser.IsDeleted)
                {
                    await bot.SendTextMessageAsync(
                        chatId: message.From.Id,
                        text: "Ваша учётная запись отключена. Обратитесь в службу поддержки.",
                        parseMode: ParseMode.Html,
                        replyMarkup: new ReplyKeyboardRemove());
                }
                //registration 2
                else if(!ruser.AccountRights.Any(x => x.RightId == AccountAccessRights.TelegramBotUsage.GetEnumGuid()))
                {
                    await mailModule.SendEmailPlainTextAsync($"<h2>Приветствуем вас</h2></br></br>Поступил запрос на подключение к телеграм боту.</br></br> Чтобы подтвердить перейдите по ссылке</br></br> <a href=\"https://nexttripto.ru/TgAccept/{ruser.Id}\" target=\"self\">перейти</a>", "Запрос на подтверждение использования телеграм бота", ruser.Email);

                    await bot.SendTextMessageAsync(
                        chatId: message.From.Id,
                        text: "На указанный вами email отправлено письмо для подтверждение доступа. Пожалуйста пройтите по ссылке в письме.",
                        parseMode: ParseMode.Html,
                        replyMarkup: new ReplyKeyboardRemove());
                }
                else
                {
                    await unitOfWork.AddEntityAsync(new DbExternalLogin
                    {
                        AccountId = ruser.Id,
                        LoginType = Common.Enums.ExternalLoginTypeEnum.Telegram,
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
                    "/menu" => bot.GetMainMenuWithInlineMenu(message.Chat.Id, unitOfWork, logger),
                    SetActive.ActionActiveEnableName => bot.SetState(message.Chat.Id, unitOfWork, true, logger),
                    SetActive.ActionActiveDisableName => bot.SetState(message.Chat.Id, unitOfWork, false, logger),
                    RequestList.ActionName => bot.GetRequestList(message.Chat.Id, unitOfWork, logger),
                    _ => bot.SendUsageInfo(message.Chat.Id, unitOfWork, logger)
                };

                Message sentMessage = await action;
                logger?.LogInformation("The message was sent with id: {sentMessageId}", sentMessage.MessageId);
            }
        }
    }
}
