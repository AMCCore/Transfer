using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.TripRequest
{
    public sealed class TripRequestSearchOrganisationDto
    {
        public Guid OrganisationId { get; set; }

        public string OrganisationName { get; set; }

        public string ContactFio { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

    }
}
