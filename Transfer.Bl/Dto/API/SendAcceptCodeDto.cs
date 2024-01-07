using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Transfer.Bl.Dto.API;

public record SendAcceptCodeDto
{
    [Required]
    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; }
}
