using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.TripRequest
{
    public sealed class TripRequestDto
    {
        public Guid Id { get; set; }

        public string? СhartererName { get; set; }

        public string AddressFrom { get; set; }

        public string AddressTo { get; set; }

        public string? ContactFio { get; set; }

        public string? ContactPhone { get; set; }

        public string? ContactEmail { get; set; }

        public DateTime TripDate { get; set; }

        public int Passengers { get; set; }

        public string? Description { get; set; }

        public string? СhartererId { get; set; }
    }
}
