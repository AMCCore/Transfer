using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.API;

public record CheckAcceptCodeDto
{
    [Required]
    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; }

    [Required]
    [JsonProperty("acceptCode")]
    public string AcceptCode { get; set; }
}
