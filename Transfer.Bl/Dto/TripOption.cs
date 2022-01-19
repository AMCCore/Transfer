using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto;

public record TripOption
{
    public Guid Id { get; init; }

    public string Name { get; init; }
}

