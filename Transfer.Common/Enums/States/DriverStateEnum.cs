using System.ComponentModel;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums.States;

public enum DriverStateEnum
{
    /// <summary>
    /// Новый
    /// </summary>
    [Description("Новый")]
    [EnumGuid("C3F42E32-C6AF-411C-BEA4-5A55FEFF6BF3")]
    New,

    /// <summary>
    /// Заблокирован
    /// </summary>
    [Description("Заблокирован")]
    [EnumGuid("D525730D-BBFB-4CEE-BA68-E6776D6890F7")]
    Blocked,

    /// <summary>
    /// На проверке
    /// </summary>
    [Description("На проверке")]
    [EnumGuid("5EC8CDBD-9681-4E4B-BE96-541E0C4F4703")]
    Checking,

    /// <summary>
    /// Проверен
    /// </summary>
    [Description("Проверен")]
    [EnumGuid("1B4D7966-DCAF-4332-ADBD-E5DE6EE77AF1")]
    Checked,
}
