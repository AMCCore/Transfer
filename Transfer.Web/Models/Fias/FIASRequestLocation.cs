using System.Runtime.Serialization;

namespace Transfer.Web.Models.Fias;

/// <summary>
/// Список ограничений / Приоритетный город поиска
/// </summary>
[DataContract]
public class FIASRequestLocation
{
    [DataMember(Name = "region")]
    public string Region { get; set; }
}