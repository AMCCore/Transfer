using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;

namespace Transfer.Dal.Entities;

[Table("TripRequests")]
public class DbTripRequest : IEntityBase, ISoftDeleteEntity, IEntityWithDateCreated
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public bool IsDeleted { get; set; }

    [MaxLength(1000)]
    public string Name { get; set; }

    public DateTime DateCreated { get; set; }

    [MaxLength(1000)]
    public string AddressFrom { get; set; }

    [MaxLength(1000)]
    public string AddressTo { get; set; }

    [MaxLength(1000)]
    public string ContactFio { get; set; }

    [MaxLength(1000)]
    public string ContactPhone { get; set; }

    [MaxLength(1000)]
    public string ContactEmail { get; set; }

    public DateTime TripDate { get; set; }

    public int Passengers { get; set; }

    public virtual ICollection<DbTripRequestOption> TripOptions { get; set; }
}
