using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.TripRequest;

public record TripRequestReplayDto
{
    public Guid OrganisationId { get; init; }

    public Guid TripRequestId { get; init; }
}
