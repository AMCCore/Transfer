﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Enums;

namespace Transfer.Dal.Entities;

[Table("OrganisationFiles")]
public class DbOrganisationFile : IEntityBase, IEntityWithDateCreated, ISoftDeleteEntity
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime DateCreated { get; set; }

    public OrganisationFileType FileType { get; set; }

    [ForeignKey(nameof(Organisation))]
    [Required]
    public Guid OrganisationId { get; set; }

    public virtual DbOrganisation Organisation { get; set; }

    [ForeignKey(nameof(Uploader))]
    [Required]
    public Guid UploaderId { get; set; }

    public virtual DbAccount Uploader { get; set; }
}
