using System;
using System.Collections.Generic;
using Transfer.Common.Enums;

namespace Transfer.Bl.Dto.TripRequest
{
    public class TripRequestSearchResultItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public long Identifier { get; set; }

        public string AddressFrom { get; set; }
        
        public string AddressTo { get; set; }
        
        public string ContactFio { get; set; }
        
        public string ContactPhone { get; set; } 
        
        public string ContactEmail { get; set; }
        
        public DateTime TripDate { get; set; }
        
        public int Passengers { get; set; }

        public IList<TripOption> TripOptions { get; set; }

        public bool Checked { get; set; }

        public Guid? Picture { get; set; }

        public int ReplaysCount { get; set; } = 0;

        public string? Description { get; set; }
    }
}