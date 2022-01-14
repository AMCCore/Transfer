using System.Collections.Generic;
using Transfer.Bl.Dto.Carrier;

namespace Transfer.Web.Models.Carrier
{
    public class СarrierSearchFilter : BaseFilterModel<СarrierSearchResultItem>
    {
        public СarrierSearchFilter(IEnumerable<СarrierSearchResultItem> list, int pageSize) : base(list, pageSize)
        {
        }

        public string Name { get; set; }
        
        public string City { get; set; }

        public bool OrderByName { get; set; }
        
        public bool OrderByRating { get; set; }
        
        public bool OrderByChecked { get; set; }
    }
}