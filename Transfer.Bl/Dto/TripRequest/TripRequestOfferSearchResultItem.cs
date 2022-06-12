using System;

namespace Transfer.Bl.Dto.TripRequest;

public sealed class TripRequestOfferSearchResultItem
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string ContactFio { get; set; }

    public string ContactPhone { get; set; }

    public string ContactEmail { get; set; }

    public string? Comment { get; set; }

    public decimal Amount { get; set; }

    public Guid CarrierId { get; set; }
}
