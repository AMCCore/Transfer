using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Enums.States;

namespace Transfer.Dal.Entities;

[Table("StateMachineStateRights")]
public class DbStateMachineStateRight : IEntityBase, IEntityWithDateCreated
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual DbStateMachineState StateMachineState { get; set; }

    [ForeignKey(nameof(StateMachineState))]
    [Required]
    public Guid StateMachineStateId { get; set; }

    public virtual DbRight Right { get; set; }

    [ForeignKey(nameof(Right))]
    [Required]
    public Guid RightId { get; set; }

    public virtual DbOrganisation Organisation { get; set; }

    [ForeignKey(nameof(Organisation))]
    public Guid? OrganisationId { get; set; }
}
