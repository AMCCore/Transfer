using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Transfer.Bot.Dtos;

namespace Transfer.Bot.Actions;

public static class SendMsgToUser
{
    public async static Task<Message> SendMessageToUser(this ITelegramBotClient bot, SendMsgToUserDto message)
    {
        return await bot.SendTextMessageAsync(
            chatId: message.ChatId,
            text: message.Message,
            replyMarkup: Menu.BaseMenu.backtomenu);
    }
}
