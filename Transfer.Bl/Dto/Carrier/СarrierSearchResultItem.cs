using System;

namespace Transfer.Bl.Dto.Carrier
{
    public class CarrierSearchResultItem
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Address { get; set; }
        
        public string ContactFio { get; set; }
        
        public string ContactPhone { get; set; } 
        
        public string ContactEmail { get; set; }

        public string ContactPosition { get; set; }

        public double Rating { get; set; }
        
        public bool Checked { get; set; }

        public Guid? Picture { get; set; }

        public bool HasTelegram { get; set; }
    }
}