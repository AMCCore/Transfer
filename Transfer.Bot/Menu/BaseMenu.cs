using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Transfer.Bot.Actions;
using Transfer.Common;

namespace Transfer.Bot.Menu
{
    internal static class BaseMenu
    {
        public const string ActionInlineName = "BackToMenu";

        public static readonly InlineKeyboardButton backToMenuButton = InlineKeyboardButton.WithCallbackData("Главное меню", ActionInlineName);

        public static readonly InlineKeyboardMarkup backtomenu = new InlineKeyboardMarkup(new[] { new[]{ backToMenuButton } });

        public static InlineKeyboardMarkup GetBasicManuInline()
        {
            var menuItems = new List<InlineKeyboardButton>();
            menuItems.Add(InlineKeyboardButton.WithCallbackData("Список актуальных заказов", RequestList.ActionInlineName));

            return new InlineKeyboardMarkup(menuItems.Select(x => new[] { x }).ToArray());
        }

        public async static Task<Message> GetMainMenuWithInlineMenu(this ITelegramBotClient bot, long Sender, IUnitOfWork unitOfWork, ILogger Logger = null)
        {
            await unitOfWork.SetState(Sender);
            
            return await bot.SendTextMessageAsync(
                chatId: Sender,
                text: "Выберите интересующую вас функцию",
                replyMarkup: GetBasicManuInline());
        }


    }
}
