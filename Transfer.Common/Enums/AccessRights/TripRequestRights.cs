using System.ComponentModel;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums.AccessRights;

public enum TripRequestRights
{
    /// <summary>
    /// Полное всеобъемлющее право на любые действия с заявками на перевозки
    /// </summary>
    [Description("Полное всеобъемлющее право на любые действия с заявками на перевозки")]
    [EnumGuid("C81A8E61-FE4D-40CF-8A44-1F1EF5C1EF6A")]
    TripRequestAdmin,


    /// <summary>
    /// Выбор перевозчика (победителя)
    /// </summary>
    [Description("Выбор перевозчика (победителя)")]
    [EnumGuid("453de109-8c41-4857-a1d2-438722da4bea")]
    CarrierChoose,
}
