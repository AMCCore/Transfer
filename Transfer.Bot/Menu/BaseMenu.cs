using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Transfer.Bot.Actions;

namespace Transfer.Bot.Menu
{
    internal static class BaseMenu
    {
        public static readonly InlineKeyboardButton backToMenuButton = InlineKeyboardButton.WithCallbackData("Главное меню", "BackToMenu");

        public static readonly InlineKeyboardMarkup backtomenu = new InlineKeyboardMarkup(new[] { new[]{ backToMenuButton } });

        public static InlineKeyboardMarkup GetBasicManuInline()
        {
            var menuItems = new List<InlineKeyboardButton>();
            menuItems.Add(InlineKeyboardButton.WithCallbackData("Список актуальных заказов", RequestList.ActionInlineName));

            return new InlineKeyboardMarkup(menuItems.Select(x => new[] { x }).ToArray());
        }
    }
}
