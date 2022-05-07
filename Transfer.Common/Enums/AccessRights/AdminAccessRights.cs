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
}
