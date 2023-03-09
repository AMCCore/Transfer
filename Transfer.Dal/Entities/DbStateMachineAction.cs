using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;
using Transfer.Common.Enums.States;

namespace Transfer.Dal.Entities;

/// <summary>
///     Переходы по статусам
/// </summary>
[Serializable]
[Table("StateMachineActions")]
public class DbStateMachineAction : IEntityBase, ISoftDeleteEntity
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public bool IsDeleted { get; set; }

    /// <summary>
    ///     Действие
    /// </summary>
    [MaxLength(1000, ErrorMessage = "\"Действие\" не может быть больше 1000 символов")]
    [Required(ErrorMessage = "\"Действие\" не может быть пустым")]
    public virtual string ActionName { get; set; }

    /// <summary>
    ///     Текст
    /// </summary>
    [MaxLength(1000, ErrorMessage = "\"Текст\" не может быть больше 1000 символов")]
    public virtual string? Description { get; set; }

    /// <summary>
    ///     Текст подтверждения
    /// </summary>
    [MaxLength(1000, ErrorMessage = "\"Текст\" не может быть больше 1000 символов")]
    public virtual string? ConfirmText { get; set; }

    /// <summary>
    ///     Код действия
    /// </summary>
    [MaxLength(1000, ErrorMessage = "\"Код действия\" не может быть больше 1000 символов")]
    [Required(ErrorMessage = "\"Код действия\" не может быть пустым")]
    public virtual string ActionCode { get; set; }

    /// <summary>
    ///     Системное действие
    /// </summary>
    [Display(Description = "Системное действие")]
    public virtual bool IsSystemAction { get; set; } = false;

    public StateMachineEnum StateMachine { get; set; }

    [ForeignKey(nameof(ToState))]
    [Required]
    public virtual Guid ToStateId { get; set; }

    [InverseProperty(nameof(DbStateMachineState.Actions))]
    public virtual DbStateMachineState ToState { get; set; }

    [InverseProperty(nameof(DbStateMachineFromStatus.StateMachineAction))]
    public virtual ICollection<DbStateMachineFromStatus> FromStates { get; set; } = new List<DbStateMachineFromStatus>();

    /// <summary>
    ///     Порядок сортировки
    /// </summary>
    [Required]
    public int SortingOrder { get; set; } = 0;
}
