using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;

namespace Transfer.Dal.Entities;

[Table("TripRequestOptions")]
public class DbTripRequestOption : IEntityBase
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public virtual DbTripRequest? TripRequest { get; set; }

    [ForeignKey(nameof(TripRequest))]
    public Guid? TripRequestId { get; set; }

    public virtual DbTripOption? TripOption { get; set; }

    [ForeignKey(nameof(TripOption))]
    public Guid? TripOptionId { get; set; }

}
