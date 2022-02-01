using System.ComponentModel;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums;

public enum DriverFileType
{
    /// <summary>
    /// Аватар
    /// </summary>
    [Description("Аватар")]
    [EnumGuid("74D9BE61-2994-43AA-ACA6-4A8E8C06591C")]
    Avatar,

    /// <summary>
    /// Водительское удостоверения
    /// </summary>
    [Description("Водительское удостоверения")]
    [EnumGuid("AACBAC7C-E85B-43B8-A308-B0C83EEDDC57")]
    License,

    /// <summary>
    /// Карта тахографа
    /// </summary>
    [Description("Карта тахографа")]
    [EnumGuid("338B5A2E-2B6D-4BAA-B9FE-9B8646F96B98")]
    TahografCard,
}

