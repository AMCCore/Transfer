using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Transfer.Bot.Dtos;

namespace Transfer.Bot.Actions;

public static class SendMsgToUser
{
    public async static Task<Message> SendMessageToUser(this ITelegramBotClient bot, SendMsgToUserDto message)
    {
        if (string.IsNullOrWhiteSpace(message.Link))
        {
            return await bot.SendTextMessageAsync(
                chatId: message.ChatId,
                text: message.Message,
                replyMarkup: Menu.BaseMenu.backtomenu);
        }

        var keyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(message.LinkName ?? message.Link, message.Link));

        return await bot.SendTextMessageAsync(
            chatId: message.ChatId,
            text: message.Message,
            replyMarkup: keyboard);
    }
}
