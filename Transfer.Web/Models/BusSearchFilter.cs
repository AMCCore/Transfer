using System.Collections.Generic;
using Transfer.Bl.Dto.Bus;

namespace Transfer.Web.Models;

public class BusSearchFilter : BaseFilterModel<BusSearchItem>
{
    public BusSearchFilter() { }

    public BusSearchFilter(IEnumerable<BusSearchItem> list, int pageSize) : base(list, pageSize) { }

    public string OrganisationName { get; set; }

    public string Make { get; set; }

    public string Model { get; set; }

    public int? Year { get; set; }

    public string TransportClass { get; set; }

    public int? PeopleCopacity { get; set; }

    public string City { get; set; }

    public IList<string> Makes { get; set; }

    public IList<string> Models { get; set; }
}

