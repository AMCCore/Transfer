using System.Collections.Generic;
using Transfer.Bl.Dto.TripRequest;

namespace Transfer.Web.Models.TripRequest;

public class RequestSearchFilter : BaseFilterModel<TripRequestSearchResultItem>
{
    public RequestSearchFilter()
    {

    }

    public RequestSearchFilter(IEnumerable<TripRequestSearchResultItem> list, int pageSize) : base(list, pageSize)
    {
    }

    public bool OrderByName { get; set; }

    public bool OrderByRating { get; set; }

    public bool OrderByChecked { get; set; }

    public bool OrderByChild { get; set; }
}