using System.Collections.Generic;
using Transfer.Bl.Dto;

namespace Transfer.Web.Models;

public class OrganisationAssetsSearchFilter : BaseFilterModel<OrganisationAssetDto>
{
    public OrganisationAssetsSearchFilter() { }

    public OrganisationAssetsSearchFilter(IEnumerable<OrganisationAssetDto> list, int pageSize) : base(list, pageSize) { }

    public OrganisationAssetType AssetType { get; set; } = OrganisationAssetType.Bus;
}