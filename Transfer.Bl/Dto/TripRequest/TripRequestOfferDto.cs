using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.TripRequest;

public class TripRequestOfferDto : TripRequestDto
{
    public string? Comment { get; set; }

    public decimal Amount { get; set; }
}
