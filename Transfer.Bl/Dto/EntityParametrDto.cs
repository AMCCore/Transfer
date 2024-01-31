using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto;

public sealed record EntityParametrDto
{
    public string Name { get; set; }

    public bool Required { get; set; }

    public string Datatype { get; set; }

    public KeyValuePair<Guid, string>[]? Variants { get; set; }
}
