﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;

namespace Transfer.Dal.Entities;

[Table("OrganisationWorkingAreas")]
public class DbOrganisationWorkingArea : IEntityBase
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    [ForeignKey(nameof(Organisation))]
    [Required]
    public Guid OrganisationId { get; set; }

    public virtual DbOrganisation Organisation { get; set; }

    [ForeignKey(nameof(Region))]
    [Required]
    public Guid RegionId { get; set; }

    public virtual DbRegion Region { get; set; }
}
