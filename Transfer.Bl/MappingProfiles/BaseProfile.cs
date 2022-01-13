using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Bl.Dto;
using Transfer.Bl.Dto.Carrier;
using Transfer.Dal.Entities;

namespace Transfer.Bl.MappingProfiles
{
    public class BaseProfile : Profile
    {
        public BaseProfile()
        {
            CreateMap<DbOrganisation, СarrierSearchResultItem>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => string.IsNullOrEmpty(o.Name) ? o.FullName : o.Name))
                .ForMember(x => x.ContactFio, opt => opt.MapFrom(o => "Евгений"))
                .ForMember(x => x.ContactEmail, opt => opt.MapFrom(o => "evgen6654@mail.ru"))
                .ForMember(x => x.ContactPhone, opt => opt.MapFrom(o => "+7 916 789 58 98"));
        }
    }
}
