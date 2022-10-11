using Microsoft.AspNetCore.Http;
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
        public const string letter = "<html><head><style>p {color: white;} .yellowlink { color: #ECBF54;} .button { cursor:pointer; text-decoration-line: none; text-decoration-color: #1B0E49; color: #1B0E49; display: block; text-align: center; line-height: 34px; width: 130px; height: 36px; background-color: #ECBF54; border-radius: 18px; } a.button { text-decoration-line: none; text-decoration-color: #1B0E49; } </style>" +
            "</head><body style=\"background: linear-gradient(89.35deg, #1B0E49 22.47%, #0F4C71 94.84%); font-family: Segoe UI, Roboto, Helvetica Neue, Arial, Noto Sans, Liberation Sans ,sans-serif, Apple Color Emoji, Segoe UI Emoji, Segoe UI Symbol, Noto Color Emoji;\"><div style=\"margin-left: 20px; margin-top: 20px; margin-left: 20px; margin-bottom: 20px;\"><div style=\"display: flex; flex-direction: column;\"><div style=\"padding: 0px;\"><img src=\"https://nexttripto.ru/images/trsite_logo.png\" style=\"display:block\"/></div><div><h2 style=\"color: white;\">Приветствуем вас</h2></div></div>" +
            "<p>Поступил запрос на подключение к телеграм боту данного адреса электронной почты.</p><p>Чтобы подтвердить подключение перейдите по ссылке <a class=\"yellowlink\" href=\"%link1%\" target=\"self\">перейти</a> или нажмите на кнопку</p><a class=\"button\" href=\"%link1%\" target=\"_self\"><div class=\"button\"><b>Подтвердить</b></div></a>" +
            "<p>При переходе по ссылке Вы подтверждаете свое согласие с условиями <b>Партнёрского соглашения</b> (<a class=\"yellowlink\" href='https://nexttripto.ru/static/5r125k' target=\"_blank\">https://nexttripto.ru/static/5r125k</a>)</p></div></body></html>";

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
                var ruser = await unitOfWork.GetSet<DbAccount>().Include(x => x.AccountRights).FirstOrDefaultAsync(x => x.Email.ToLower() == message.Text.ToLower());
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
                    await mailModule.SendEmailPlainTextAsync(letter.Replace("%link1%", $"https://nexttripto.ru/TgAccept/{ruser.Id}"), "Запрос на подтверждение использования телеграм бота", ruser.Email, true);

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
