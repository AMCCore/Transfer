using System;
using System.Collections.Generic;
using Transfer.Common.Enums.States;

namespace Transfer.Bl.Dto;

public class SetNextStatesDto
{
    public StateMachineDto Model { get; set; }

    public StateMachineEnum StateMachine { get; set; }

    public ICollection<Guid?> OrganisationIds { get; set; } = new List<Guid?>();
}
