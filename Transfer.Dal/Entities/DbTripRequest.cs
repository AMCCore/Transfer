using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;
using Transfer.Common.Enums.States;

namespace Transfer.Dal.Entities;

[Table("TripRequests")]
public class DbTripRequest : IEntityBase, ISoftDeleteEntity, IEntityWithDateCreated
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime DateCreated { get; set; }

    [MaxLength(1000)]
    public string? СhartererName { get; set; }

    [MaxLength(1000)]
    public string AddressFrom { get; set; }

    [MaxLength(1000)]
    public string AddressTo { get; set; }

    [MaxLength(1000)]
    public string? ContactFio { get; set; }

    [MaxLength(1000)]
    public string? ContactPhone { get; set; }

    [MaxLength(1000)]
    public string? ContactEmail { get; set; }

    public DateTime TripDate { get; set; }

    public int Passengers { get; set; }

    public virtual ICollection<DbTripRequestOption> TripOptions { get; set; } = new List<DbTripRequestOption>();

    public virtual ICollection<DbTripRequestReplay> TripRequestReplays { get; set; } = new List<DbTripRequestReplay>();

    public virtual ICollection<DbTripRequestOffer> TripRequestOffers { get; set; } = new List<DbTripRequestOffer>();

    public virtual DbOrganisation? Charterer { get; set; }

    [ForeignKey(nameof(Charterer))]
    public Guid? ChartererId { get; set; }

    public string? Description { get; set; }

    public int? LuggageVolume { get; set; }

    [Required]
    public Guid State { get; set; }

    public virtual DbRegion? RegionFrom { get; set; }

    [ForeignKey(nameof(RegionFrom))]
    public Guid? RegionFromId { get; set; }

    public virtual DbRegion? RegionTo { get; set; }

    [ForeignKey(nameof(RegionTo))]
    public Guid? RegionToId { get; set; }

    public virtual ICollection<DbTripRequestIdentifier> Identifiers { get; set; } = new List<DbTripRequestIdentifier>();
}
