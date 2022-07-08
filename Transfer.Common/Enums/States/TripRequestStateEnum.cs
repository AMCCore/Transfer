using System.ComponentModel;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums.States;

public enum TripRequestStateEnum
{
    /// <summary>
    /// Действующий
    /// </summary>
    [Description("Действующий")]
    [EnumGuid("C2BD98B2-CA6E-4C3C-BD75-8D10FD804DA4")]
    Active,

    /// <summary>
    /// Отменён
    /// </summary>
    [Description("Отменён")]
    [EnumGuid("4DB45853-47F0-46DB-A4FE-CC984B5E038A")]
    Canceled,

    /// <summary>
    /// Перемещен в архив
    /// </summary>
    [Description("Перемещен в архив")]
    [EnumGuid("3DC01BD1-7C95-4F16-B7A9-2EFB86AC7A0E")]
    Archived,

    /// <summary>
    /// Просрочен
    /// </summary>
    [Description("Просрочен")]
    [EnumGuid("3857D8C6-BECE-4B39-BF37-3A04A550E093")]
    Overdue,

    /// <summary>
    /// Перевозчик выбран
    /// </summary>
    [Description("Перевозчик выбран (заключение договора)")]
    [EnumGuid("63D8EB2C-ADF9-4208-81C9-7EF17C0A905C")]
    CarrierSelected,

    /// <summary>
    /// Завершен
    /// </summary>
    [Description("Завершен")]
    [EnumGuid("BA47C059-2686-4D6D-967E-DB36334A36D4")]
    Completed,
}
