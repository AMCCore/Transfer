using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;
using Transfer.Common.Enums.States;

namespace Transfer.Dal.Entities;

/// <summary>
///     Источник перехода
/// </summary>
[Serializable]
[Table("StateMachineFromStatuses")]
public class DbStateMachineFromStatus : IEntityBase, ISoftDeleteEntity
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public bool IsDeleted { get; set; }

    /// <summary>
    ///     Код права
    /// </summary>
    [Display(Description = "Код права")]
    public virtual Guid? RightCode { get; set; }

    [ForeignKey(nameof(StateMachineAction))]
    public virtual Guid? StateMachineActionId { get; set; }

    [InverseProperty(nameof(DbStateMachineAction.FromStates))]
    public virtual DbStateMachineAction StateMachineAction { get; set; }

    [ForeignKey(nameof(FromState))]
    public virtual Guid? FromStateId { get; set; }

    public virtual DbStateMachineState FromState { get; set; }

    public StateMachineEnum StateMachine { get; set; }
}
