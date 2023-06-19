namespace Transfer.Bot.Dtos;

public class SendMsgToUserDto
{
    public long ChatId { get; set; }

    public string Message { get; set; }

    public string? Link { get; set; }

    public string? LinkName { get; set; }

    public bool NeedMenu { get; set; } = true;
}