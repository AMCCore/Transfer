using System;

namespace Transfer.Web.Models.Carrier
{
    public class СarrierSearchResultItem
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Address { get; set; }
        
        public string ContactFio { get; set; }
        
        public string ContactPhone { get; set; } 
        
        public string ContactEmail { get; set; }
        
        public double Rating { get; set; }
        
        public bool Checked { get; set; }
    }
}