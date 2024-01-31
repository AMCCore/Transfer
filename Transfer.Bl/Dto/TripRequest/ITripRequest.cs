using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common.Attributes;

namespace Transfer.Bl.Dto.TripRequest;

public interface ITripRequest
{
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

    public Guid? OrgCreatorId { get; set; }

    public bool ChildTrip { get; set; }

    public bool StandTrip { get; set; }

    public int PaymentType { get; set; }
}
