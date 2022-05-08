using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.TripRequest
{
    public sealed class TripRequestDto : StateMachineDto
    {
        public Guid Id { get; set; }

        public string ChartererName { get; set; }

        public string AddressFrom { get; set; }

        public string? AddressFromFias { get; set; }

        public string AddressTo { get; set; }

        public string? AddressToFias { get; set; }

        public string? ContactFio { get; set; }

        public string ContactPhone { get; set; }

        public string? ContactEmail { get; set; }

        public DateTime TripDate { get; set; }

        public int Passengers { get; set; }

        public int? LuggageVolume { get; set; }

        public string? Description { get; set; }

        public Guid? ChartererId { get; set; }

        public bool ChildTrip { get; set; }

        public bool StandTrip { get; set; }

        public int PaymentType { get; set; }

        public long LastUpdateTick { get; set; }

        [Required]
        public override Guid State { get; set; }

        public Guid? RegionToId { get; set; }

        public Guid? RegionFromId { get; set; }

        public string? RegionToName { get; set; }

        public string? RegionFromName { get; set; }
    }
}
