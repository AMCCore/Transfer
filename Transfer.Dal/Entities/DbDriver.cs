﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;
using Transfer.Common.Enums.States;

namespace Transfer.Dal.Entities;

[Table("Drivers")]
public class DbDriver : IEntityBase, ISoftDeleteEntity, IEntityWithDateCreated
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime DateCreated { get; set; }

    [ForeignKey(nameof(Creator))]
    public Guid? CreatorId { get; set; }

    public virtual DbAccount? Creator { get; set; }

    public virtual DbOrganisation? Organisation { get; set; }
    
    [ForeignKey(nameof(Organisation))]
    public Guid? OrganisationId { get; set; }

    [MaxLength(100)]
    public string? TelegramId { get; set; }
    
    public virtual ICollection<DbDriversLicense> DbDriversLicenses { get; set; } = new List<DbDriversLicense>();

    public virtual ICollection<DbDriverFile> DriverFiles { get; set; } = new List<DbDriverFile>();

    public virtual DbPersonData? PersonData { get; set; }

    [ForeignKey(nameof(PersonData))]
    public Guid? PersonDataId { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string LastName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    [MaxLength(1000)]
    public string MiddleName { get; set; }

    /// <summary>
    /// EMail
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string EMail { get; set; }

    /// <summary>
    /// Phone
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string Phone { get; set; }

    public DriverStateEnum State { get; set; } = DriverStateEnum.Checked;
}
