using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;

namespace Transfer.Dal.Entities;

[Table("HistoryLogs")]
public class DbHistoryLog : IEntityBase, IEntityWithDateCreated
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public DateTime DateCreated { get; set; }

    [Required]
    public Guid AccountId { get; set; }

    [Required]
    public Guid EntityId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ActionName { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }
}