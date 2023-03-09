using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;

namespace Transfer.Dal.Entities;

[Table("Regions")]
public class DbRegion : IEntityBase
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    [MaxLength(1000)]
    public string Name { get; set; }

    public virtual ICollection<DbOrganisationWorkingArea> WorkingArea { get; set; } = new List<DbOrganisationWorkingArea>();

    /// <summary>
    ///     Родительский регион
    /// </summary>
    [ForeignKey(nameof(ParrentRegion))]
    public Guid? ParrentRegionId { get; set; }

    /// <summary>
    ///     Родительский регион
    /// </summary>
    public virtual DbRegion? ParrentRegion { get; set; }

    public virtual ICollection<DbRegion> ChildRegions { get; set; } = new List<DbRegion>();

    [InverseProperty(nameof(DbTripRequest.RegionFrom))]
    public virtual ICollection<DbTripRequest> TripRequestRegionsFrom { get; set; } = new List<DbTripRequest>();

    [InverseProperty(nameof(DbTripRequest.RegionTo))]
    public virtual ICollection<DbTripRequest> TripRequestRegionsTo { get; set; } = new List<DbTripRequest>();

    public virtual ICollection<DbRegionAlias> RegionAlias { get; set; } = new List<DbRegionAlias>();
}
