using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto;

public struct NextStateDto
{
    public Guid NextStateId { get; set; }

    public string ButtonName { get; set; }

    public string? ConfirmText { get; set; }
}
