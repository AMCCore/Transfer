using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.Organisation;

public sealed class OrganisationAccountDto : AccountDto
{
    public Guid OrganisationId { get; set; }

    public string? Phone { get; set; }
}
