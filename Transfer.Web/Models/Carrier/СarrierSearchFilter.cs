using System.Collections.Generic;
using Transfer.Bl.Dto.Carrier;

namespace Transfer.Web.Models.Carrier;

public class CarrierSearchFilter : BaseFilterModel<CarrierSearchResultItem>
{
    public CarrierSearchFilter() { }

    public CarrierSearchFilter(IEnumerable<CarrierSearchResultItem> list, int pageSize) : base(list, pageSize)
    {
    }

    public string Phone { get; set; }

    public string InnName { get; set; }

    public string City { get; set; }

    public bool OrderByName { get; set; }

    public bool OrderByRating { get; set; }

    public bool OrderByChecked { get; set; }

    public bool ActiveOnly { get; set; } = true;
}