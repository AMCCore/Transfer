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
    [EnumGuid("60480B00-CFA0-4ECF-8875-33340C5EC397")]
    TO,

    /// <summary>
    /// ОСАГО
    /// </summary>
    [Description("ОСАГО")]
    [EnumGuid("124246B1-3168-4FEF-BE2F-71F4D25B931C")]
    Inshurance,

    /// <summary>
    /// ОСГОП
    /// </summary>
    [Description("ОСГОП")]
    [EnumGuid("B6C27510-6179-4F50-8537-D749C335CFB3")]
    Osgop,

    /// <summary>
    /// Калибровка тахографа
    /// </summary>
    [Description("Калибровка тахографа")]
    [EnumGuid("DC61766B-F37A-4AC9-AD30-E950DCE55BEB")]
    Tahograf,

    /// <summary>
    /// Главняа фотография
    /// </summary>
    [Description("Калибровка тахографа")]
    [EnumGuid("525AB103-DD52-4B30-8F01-EFB31F19064B")]
    PhotoMain,
}