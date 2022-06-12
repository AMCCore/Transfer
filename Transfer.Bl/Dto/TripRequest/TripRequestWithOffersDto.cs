using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.TripRequest;

public sealed class TripRequestWithOffersDto : TripRequestDto
{
    public IList<TripOption> TripOptions { get; set; }

    public IList<TripRequestOfferSearchResultItem> Offers { get; set; } = new List<TripRequestOfferSearchResultItem>(0);
}
