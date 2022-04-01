using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Transfer.Bot.Menu;
using Transfer.Common;
using Transfer.Dal.Entities;

namespace Transfer.Bot.Actions
{
    internal static class SetActive
    {
        public const string ActionActiveEnableName = "/enable";
        public const string ActionActiveDisableName = "/disable";

        public async static Task<Message> SetState(this ITelegramBotClient bot, long Sender, IUnitOfWork unitOfWork, bool IsActive, ILogger Logger = null)
        {
            var state = await unitOfWork.GetSet<DbTgActionState>().Where(x => !x.Account.IsDeleted && x.Account.ExternalLogins.Any(a => !a.IsDeleted && a.LoginType == Common.Enums.ExternalLoginEnum.Telegram && a.Value == Sender.ToString())).FirstAsync();
            state.IsActive = IsActive;
            await unitOfWork.SaveChangesAsync();

            return await bot.SendTextMessageAsync(
                chatId: Sender,
                text: IsActive ? "Уведомления включены" : "Уведомления временно выключены",
                replyMarkup: BaseMenu.backtomenu);
        }
    }
}
