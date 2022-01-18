using System.Collections.Generic;
using Transfer.Bl.Dto.Request;

namespace Transfer.Web.Models.Request
{
    public class RequestSearchFilter : BaseFilterModel<RequestSearchResultItem>
    {
        public RequestSearchFilter()
        {

        }

        public RequestSearchFilter(IEnumerable<RequestSearchResultItem> list, int pageSize) : base(list, pageSize)
        {
        }
        
        public bool OrderByName { get; set; }
        
        public bool OrderByRating { get; set; }
        
        public bool OrderByChild { get; set; }
    }
}