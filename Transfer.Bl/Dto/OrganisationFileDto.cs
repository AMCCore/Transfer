using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common.Enums;

namespace Transfer.Bl.Dto;

public class OrganisationFileDto
{
    public Guid Id { get; set; }

    public Guid FileId { get; set; }

    public OrganisationFileType FileType { get; set; }
}
