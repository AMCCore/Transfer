using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;
using Transfer.Common.Enums;
using System;

namespace Transfer.Dal.Entities;

[Table("TgActionStates")]
public class DbTgActionState : IEntityBase
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    [ForeignKey(nameof(Account))]
    public Guid? AccountId { get; set; }

    public virtual DbAccount Account { get; set; }

    [MaxLength(255)]
    [Required]
    public string State { get; set; }

    [MaxLength(2000)]
    public string? StateProps { get; set; }

    public bool IsActive { get; set; } = true;
}
