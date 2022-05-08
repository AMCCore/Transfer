using System.Runtime.Serialization;

namespace Transfer.Web.Models.Fias;

[DataContract]
internal struct FIASRequest
{
    /// <summary>
    /// текст запроса
    /// </summary>
    [DataMember(Name = "query")]
    public string Query { get; set; }

    /// <summary>
    /// кол-во возвращаемых результатов
    /// </summary>
    [DataMember(Name = "count")]
    public int Count { get; set; }

    /// <summary>
    /// Список ограничений
    /// </summary>
    [DataMember(Name = "locations")]
    public FIASRequestLocation[] Locations { get; set; }

    /// <summary>
    /// Приоритетный город поиска
    /// </summary>
    [DataMember(Name = "locations_boost")]
    public FIASRequestLocation[] LocationsBoost { get; set; }
}
