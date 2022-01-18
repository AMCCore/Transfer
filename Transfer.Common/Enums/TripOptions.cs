using System.ComponentModel;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums;

public enum TripOptions
{
    /// <summary>
    /// Услуги гида
    /// </summary>
    [Description("Услуги гида")]
    [EnumGuid("FA7CF9B0-C67C-408A-B851-57AB20EBEDC6")]
    TripGuide,
    
    /// <summary>
    /// Оплата картой
    /// </summary>
    [Description("Оплата картой")]
    [EnumGuid("2C153FD2-1481-4656-9070-3EF907B757B9")]
    CardPayment,

  
    /// <summary>
    /// Оплата наличными
    /// </summary>
    [Description("Оплата наличными")]
    [EnumGuid("8A83799A-37B4-40C6-B51B-C647AEF94A48")]
    CashPayment,
    
    /// <summary>
    /// Поездка с простоем
    /// </summary>
    [Description("Поездка с простоем")]
    [EnumGuid("14E0B630-D056-4CFC-88A8-0701681EC733")]
    IdleTrip,

    /// <summary>
    /// Перевозка детей
    /// </summary>
    [Description("Перевозка детей")]
    [EnumGuid("B7119E8D-08F6-4182-9025-77F158A2B0E3")]
    ChildTrip,

}