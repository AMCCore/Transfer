using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;
using Transfer.Common.Enums.States;

namespace Transfer.Dal.Entities;

/// <summary>
///     Статус
/// </summary>
[Serializable]
[Table("StateMachineStates")]
public class DbStateMachineState : IEntityBase, ISoftDeleteEntity
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }
    public bool IsDeleted { get; set; }

    [InverseProperty(nameof(DbStateMachineAction.ToState))]
    public virtual ICollection<DbStateMachineAction> Actions { get; set; } = new List<DbStateMachineAction>();

    /// <summary>
    ///     Наименование
    /// </summary>
    [Display(Description = "Наименование")]
    [MaxLength(1000, ErrorMessage = "\"Наименование\" не может быть больше 1000 символов")]
    public virtual string Name { get; set; }

    public StateMachineEnum StateMachine { get; set; }
}
