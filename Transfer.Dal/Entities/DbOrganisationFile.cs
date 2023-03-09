using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    public OrganisationFileTypeEnum FileType { get; set; }

    [ForeignKey(nameof(Uploader))]
    [Required]
    public Guid UploaderId { get; set; }

    public virtual DbAccount Uploader { get; set; }

    [ForeignKey(nameof(Organisation))]
    [Required]
    public Guid OrganisationId { get; set; }

    public virtual DbOrganisation Organisation { get; set; }

    [ForeignKey(nameof(File))]
    [Required]
    public Guid FileId { get; set; }

    public virtual DbFile File { get; set; }


}

