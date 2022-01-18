using System;

namespace Transfer.Bl.Dto.Request
{
    public class RequestSearchResultItem
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string AddressFrom { get; set; }
        
        public string AddressTo { get; set; }
        
        public string ContactFio { get; set; }
        
        public string ContactPhone { get; set; } 
        
        public string ContactEmail { get; set; }
        
        public DateTime TripDate { get; set; }
        
        public int Passengers { get; set; }

    }
}