using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;
using Transfer.Common.Enums;

namespace Transfer.Dal.Entities;

[Table("BusFiles")]
public class DbBusFile : IEntityBase, IEntityWithDateCreated, ISoftDeleteEntity
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime DateCreated { get; set; }

    public BusFileTypeEnum FileType { get; set; }

    [ForeignKey(nameof(Uploader))]
    [Required]
    public Guid UploaderId { get; set; }

    public virtual DbAccount Uploader { get; set; }

    [ForeignKey(nameof(Bus))]
    [Required]
    public Guid BusId { get; set; }

    public virtual DbBus Bus { get; set; }

    [ForeignKey(nameof(File))]
    [Required]
    public Guid FileId { get; set; }

    public virtual DbFile File { get; set; }
}
