﻿using System;
using System.Collections.Generic;
using Transfer.Common.Enums;

namespace Transfer.Bl.Dto.TripRequest
{
    public class TripRequestSearchResultItem
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

        public ICollection<TripOption> TripOptions { get; set; }

        public bool Checked { get; set; }
    }
}