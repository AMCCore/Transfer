using System.ComponentModel;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums.AccessRights;

public enum AdminAccessRights
{
    /// <summary>
    /// Полное всеобъемлющее право на любые действия
    /// </summary>
    [Description("Полное всеобъемлющее право на любые действия")]
    [EnumGuid("ECF45E41-F532-49A3-84CE-58E7212BBCCC")]
    IsAdmin,

    /// <summary>
    /// Право получать все уведомления в бот
    /// </summary>
    [Description("Право получать все уведомления в бот")]
    [EnumGuid("72BBB5F3-B036-4B84-9F58-08712E1BE695")]
    BotNotifications,

}
