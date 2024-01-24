using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.API.Geotree;

public sealed record GeoDataDto
{
    [JsonProperty("lon")]
    public decimal Lon { get; set; }

    [JsonProperty("lat")]
    public decimal Lat { get; set; }
}
