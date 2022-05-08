using System.ComponentModel;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums.AccessRights;

public enum AccountAccessRights
{
    /// <summary>
    /// Разрешено использование телеграм бота
    /// </summary>
    [Description("Разрешено использование телеграм бота")]
    [EnumGuid("66432E05-2700-4EFD-A177-1DFF14D85901")]
    TelegramBotUsage,
}
