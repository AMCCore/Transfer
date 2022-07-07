using System.ComponentModel;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums.States;

public enum OrganisationStateEnum
{
    /// <summary>
    /// Новый
    /// </summary>
    [Description("Новый")]
    [EnumGuid("BFC18692-FDBF-4A41-9998-78DDD74A4F54")]
    New,

    /// <summary>
    /// Заблокирован
    /// </summary>
    [Description("Заблокирован")]
    [EnumGuid("FB7DAB75-7781-4F1F-8D63-479C77CFE1F0")]
    Blocked,

    /// <summary>
    /// На проверке
    /// </summary>
    [Description("На проверке")]
    [EnumGuid("D408D750-1B9F-4C90-B1FB-25300DF42981")]
    Checking,

    /// <summary>
    /// Проверен
    /// </summary>
    [Description("Проверен")]
    [EnumGuid("9AEC0B81-230D-4137-B4DD-A1B51B5EB467")]
    Checked,
}
