using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.Organisation;

public sealed class OrganisationAccountDto : AccountDto
{
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public Guid OrganisationId { get; set; }

    public string OrganisationName { get; set; }

    public string? Phone { get; set; }

    public bool IsDriver { get; set; } = false;
}
