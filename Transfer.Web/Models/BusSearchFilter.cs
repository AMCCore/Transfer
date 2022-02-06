using System.Collections.Generic;
using Transfer.Bl.Dto;

namespace Transfer.Web.Models
{
    public class BusSearchFilter : BaseFilterModel<OrganisationAssetDto>
    {
        public BusSearchFilter() { }

        public BusSearchFilter(IEnumerable<OrganisationAssetDto> list, int pageSize) : base(list, pageSize) { }

        public string OrganisationName { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public int? Year { get; set; }

        public string TransportClass { get; set; }

        public int? PeopleCopacity { get; set; }

        public string City { get; set; }

    }
}
