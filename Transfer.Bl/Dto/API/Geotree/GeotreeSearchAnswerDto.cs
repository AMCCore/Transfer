using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.API.Geotree;

public sealed record GeotreeSearchAnswerDto
{
    [JsonProperty("value")]
    public string Value { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("oktmo_short")]
    public string OktmoShort { get; set; }

    [JsonProperty("oktmo")]
    public string Oktmo { get; set; }

    [JsonProperty("name_display")]
    public string NameDisplay { get; set; }

    [JsonProperty("name_source")]
    public string NameSource { get; set; }

    [JsonProperty("name_without_type")]
    public string NameWithoutType { get; set; }

    [JsonProperty("name_type_short")]
    public string NameTypeShort { get; set; }

    [JsonProperty("name_type_full")]
    public string NameTypeFull { get; set; }

    [JsonProperty("level")]
    public string Level { get; set; }

    [JsonProperty("oktmo_type")]
    public string OktmoType { get; set; }

    [JsonProperty("center_level")]
    public string CenterLevel { get; set; }

    [JsonProperty("unique_display")]
    public int UniqueDisplay { get; set; }

    [JsonProperty("unique_without_type")]
    public int UniqueWithoutType { get; set; }

    [JsonProperty("unique_source")]
    public int UniqueSource { get; set; }

    [JsonProperty("geo_type")]
    public string GeoType { get; set; }

    [JsonProperty("siblings")]
    public string Siblings { get; set; }

    [JsonProperty("parent_level")]
    public string ParentLevel { get; set; }

    [JsonProperty("population")]
    public string Population { get; set; }

    [JsonProperty("center_of")]
    public string CenterOf { get; set; }

    [JsonProperty("area")]
    public double Area { get; set; }

    [JsonProperty("geo_center")]
    public GeoDataDto GeoCenter { get; set; }

    [JsonProperty("geo_center_inside")]
    public bool GeoCenterInside { get; set; }

    [JsonProperty("geo_inside")]
    public GeoDataDto GeoInside { get; set; }

}
