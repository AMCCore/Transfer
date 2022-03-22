using System.ComponentModel;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums;

public enum BusStateEnum
{
    /// <summary>
    /// Новый
    /// </summary>
    [Description("Новый")]
    [EnumGuid("AE78EBC8-A108-4CAD-A512-EFA47F69F537")]
    New,

    /// <summary>
    /// Заблокирован
    /// </summary>
    [Description("Заблокирован")]
    [EnumGuid("05E066AD-85C3-4221-BF05-BC549848742C")]
    Blocked,

    /// <summary>
    /// На проверке
    /// </summary>
    [Description("На проверке")]
    [EnumGuid("DA1ED977-F9D9-4036-94E3-1D828EC83A9E")]
    Checking,

    /// <summary>
    /// Проверен
    /// </summary>
    [Description("Проверен")]
    [EnumGuid("24554358-C796-4355-A9FD-34052A66A8CD")]
    Checked,
}
