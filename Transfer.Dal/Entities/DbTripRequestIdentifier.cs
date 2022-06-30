using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;

namespace Transfer.Dal.Entities;

[Table("TripRequestIdentifiers")]
public class DbTripRequestIdentifier : IEntityBase
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Identifier { get; set; }

    public virtual DbTripRequest TripRequest { get; set; }

    [ForeignKey(nameof(TripRequest))]
    [Required]
    public Guid TripRequestId { get; set; }
}
