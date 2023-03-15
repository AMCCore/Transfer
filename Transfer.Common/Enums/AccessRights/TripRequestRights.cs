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
    /// Получать уведомления о норвых заказах в Телеграм
    /// </summary>
    [Description("Получать уведомления о норвых заказах в Телеграм")]
    [EnumGuid("ee1319a4-47a4-4635-94f7-d5174d940178")]
    TripRequestNewRequestTelegramPush,

    /// <summary>
    /// Просмотр заказов (списка)
    /// </summary>
    [Description("Просмотр заказов (списка)")]
    [EnumGuid("8eb843c2-890c-40e3-b7dc-b7e7880bf09c")]
    TripRequestView,

    /// <summary>
    /// Создание заказа
    /// </summary>
    [Description("Создание заказа")]
    [EnumGuid("a1cb4fb8-34df-43d6-932a-cdbc326a981b")]
    TripRequestCreate,

    /// <summary>
    /// Отклик на заказ
    /// </summary>
    [Description("Отклик на заказ")]
    [EnumGuid("9e57abf5-06fa-4fa7-ad1e-3bedf7c425b9")]
    TripRequestMakeOffer,

    /// <summary>
    /// Выбор перевозчика (победителя)
    /// </summary>
    [Description("Выбор перевозчика (победителя)")]
    [EnumGuid("453de109-8c41-4857-a1d2-438722da4bea")]
    CarrierChoose,
}
