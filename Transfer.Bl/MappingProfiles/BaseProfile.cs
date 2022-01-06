using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Bl.Dto;

namespace Transfer.Bl.MappingProfiles
{
    public class BaseProfile : Profile
    {
        public BaseProfile()
        {
            CreateMap<Foo, Bar>();
            CreateMap<Bar, Foo>();
        }
    }
}
