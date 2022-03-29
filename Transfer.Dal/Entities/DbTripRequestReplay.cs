using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;

namespace Transfer.Dal.Entities;

[Table("TripRequestReplays")]
public class DbTripRequestReplay : IEntityBase
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public virtual DbTripRequest TripRequest { get; set; }

    [ForeignKey(nameof(TripRequest))]
    [Required]
    public Guid TripRequestId { get; set; }

    [Column(TypeName = "money")]
    [Required]
    public decimal Amount { get; set; } = 0;

    public virtual DbOrganisation Carrier { get; set; }

    [ForeignKey(nameof(Carrier))]
    [Required]
    public Guid CarrierId { get; set; }

}
