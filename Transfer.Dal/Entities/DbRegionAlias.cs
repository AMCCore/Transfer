using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;

namespace Transfer.Dal.Entities;

[Table("RegionAlias")]
public class DbRegionAlias : IEntityBase
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    [MaxLength(1000)]
    public string Name { get; set; }

    /// <summary>
    ///     Родительский регион
    /// </summary>
    [ForeignKey(nameof(ParrentRegion))]
    public Guid? ParrentRegionId { get; set; }

    /// <summary>
    ///     Родительский регион
    /// </summary>
    public virtual DbRegion? ParrentRegion { get; set; }
}

