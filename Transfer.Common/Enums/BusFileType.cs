using System.ComponentModel;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums;

public enum BusFileType
{
    /// <summary>
    /// Photo
    /// </summary>
    [Description("Фотография")]
    [EnumGuid("0028E033-EC72-487E-83C6-6D164F5DA715")]
    Photo,

    /// <summary>
    /// СТС
    /// </summary>
    [Description("СТС")]
    [EnumGuid("A5BCD729-7BA9-4647-B4F2-27C87B69C490")]
    Reg,

    /// <summary>
    /// Техосмотр
    /// </summary>
    [Description("Техосмотр")]
    [EnumGuid("A5BCD729-7BA9-4647-B4F2-27C87B69C490")]
    TO,

    /// <summary>
    /// ОСАГО
    /// </summary>
    [Description("ОСАГО")]
    [EnumGuid("A5BCD729-7BA9-4647-B4F2-27C87B69C490")]
    Inshurance,

    /// <summary>
    /// ОСГОП
    /// </summary>
    [Description("ОСГОП")]
    [EnumGuid("A5BCD729-7BA9-4647-B4F2-27C87B69C490")]
    Osgop,

    /// <summary>
    /// Калибровка тахографа
    /// </summary>
    [Description("Калибровка тахографа")]
    [EnumGuid("A5BCD729-7BA9-4647-B4F2-27C87B69C490")]
    Tahograf,
}
