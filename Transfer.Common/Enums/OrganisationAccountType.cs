using System.ComponentModel;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums;

public enum OrganisationAccountType
{
    /// <summary>
    /// Директор (основное контактное лицо)
    /// </summary>
    [Description("Директор (основное контактное лицо)")]
    [EnumGuid("A829216D-14E3-466D-9484-072E1D8E4DD6")]
    Director,

    /// <summary>
    /// Оператор
    /// </summary>
    [Description("Оператор")]
    [EnumGuid("FBAD3F45-2D85-4A94-975D-88619ED0ACBC")]
    Operator
}