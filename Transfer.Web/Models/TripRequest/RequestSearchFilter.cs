using System.Collections.Generic;
using Transfer.Bl.Dto.TripRequest;
using Transfer.Web.Models.Enums;

namespace Transfer.Web.Models.TripRequest;

public class RequestSearchFilter : BaseFilterModel<TripRequestSearchResultItem>
{
    public RequestSearchFilter()
    {

    }

    public RequestSearchFilter(IEnumerable<TripRequestSearchResultItem> list, int pageSize) : base(list, pageSize)
    {
    }

    public int OrderBy { get; set; } = (int)TripRequestSearchOrderEnum.OrderByDateStartAsc;

    public int State { get; set; } = (int)TripRequestSearchStateEnum.StateNew;

    public bool MyRequests { get; set; } = false;

}