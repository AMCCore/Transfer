using System.ComponentModel;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums.States;

public enum StateMachineEnum
{
    /// <summary>
    /// Запрос на перевозку
    /// </summary>
    [Description("Запрос на перевозку")]
    [EnumGuid("E451122C-0A91-4A64-8983-6CADB9FF05B4")]
    TripRequest,

    /// <summary>
    /// Организация
    /// </summary>
    [Description("Организация")]
    [EnumGuid("3F0B5B34-EF9C-41E0-8D74-99B4BBC42CB7")]
    Organisation,

    /// <summary>
    /// Водитель
    /// </summary>
    [Description("Водитель")]
    [EnumGuid("A83508E1-C986-46C7-A2B5-A93F3CACAC0B")]
    Driver,

    /// <summary>
    /// Транспортное средство
    /// </summary>
    [Description("Транспортное средство")]
    [EnumGuid("51A2FFF9-FB4D-456A-A30A-DFFB6684D80D")]
    Bus,
}
