using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Transfer.Bl.Dto.API;

public class SendAcceptCodeDto
{
    [Required]
    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; }
}
