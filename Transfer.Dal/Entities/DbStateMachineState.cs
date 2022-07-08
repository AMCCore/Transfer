using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;
using Transfer.Common.Enums.States;

namespace Transfer.Dal.Entities;

[Table("StateMachineStates")]
public class DbStateMachineState : IEntityBase, IEntityWithDateCreated
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public DateTime DateCreated { get; set; }

    public StateMachineEnum StateMachine { get; set; }

    public Guid StateFrom { get; set; }

    public Guid StateTo { get; set; }

    public bool UseByOwner { get; set; } = false;

    public bool UseByAuthorized { get; set; } = true;

    public bool UseByOrganisation { get; set; } = false;

    public bool UseBySystem { get; set; } = false;

    public virtual ICollection<DbStateMachineStateRight> StateMachineStateRights { get; set; } = new List<DbStateMachineStateRight>();

    [MaxLength(100)]
    public string Description { get; set; }

    [MaxLength(1000)]
    public string? ButtonName { get; set; }

    [MaxLength(1000)]
    public string? ConfirmText { get; set; }
}
