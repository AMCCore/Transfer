using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.TripRequest;

public class TripRequestOfferDto : TripRequestDto
{
    public string? Comment { get; set; }

    [Required]
    public decimal Amount { get; set; }

    public IList<TripOption> TripOptions { get; set; }

    public Guid CarrierId { get; set; }
}
