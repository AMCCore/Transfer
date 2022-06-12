using System;

namespace Transfer.Bot.Dtos;

public struct SendMsgToUserDto
{
    public long ChatId { get; set; }

    public string Message { get; set; }

    public string? Link { get; set; }

    public string? LinkName { get; set; }
}