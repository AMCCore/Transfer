using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto
{
    public class OrganisationAssetDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? Picture { get; set; }

        public string TransportClass { get; set; }

        public int? PeopleCopacity { get; set; }

        public string Phone { get; set; }

        public string EMail { get; set; }
    }
}
