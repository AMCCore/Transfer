using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;
using Transfer.Common.Enums;

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

    public virtual ICollection<DbTripRequestOption> TripOptions { get; set; }

    public virtual DbOrganisation? Charterer { get; set; }

    [ForeignKey(nameof(Charterer))]
    public Guid? ChartererId { get; set; }

    public string? Description { get; set; }

    public int? LuggageVolume { get; set; }

    public TripRequestStateEnum State { get; set; } = TripRequestStateEnum.New;
}
