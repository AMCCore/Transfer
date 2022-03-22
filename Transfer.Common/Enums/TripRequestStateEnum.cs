using System.ComponentModel;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums;

public enum TripRequestStateEnum
{
    /// <summary>
    /// Новый
    /// </summary>
    [Description("Новый")]
    [EnumGuid("D18D3114-582F-4E30-A1E2-8E84A9F7F31B")]
    New,
    
    /// <summary>
    /// Завершен сбор предложений
    /// </summary>
    [Description("Завершен сбор предложений")]
    [EnumGuid("6F8AE582-7FFA-4F29-91E0-EED64D142EA6")]
    ProposalsComplete,

    /// <summary>
    /// Отменён
    /// </summary>
    [Description("Отменён")]
    [EnumGuid("4DB45853-47F0-46DB-A4FE-CC984B5E038A")]
    Canceled,

    /// <summary>
    /// Отправлен в ЕРП
    /// </summary>
    [Description("Отправлен в ЕРП")]
    [EnumGuid("89F977CD-6B08-4BC0-9D0B-42DB21F320C3")]
    InErp,
}
