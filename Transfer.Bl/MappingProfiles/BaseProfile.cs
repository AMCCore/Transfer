using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Bl.Dto;
using Transfer.Bl.Dto.Carrier;
using Transfer.Bl.Dto.TripRequest;
using Transfer.Dal.Entities;

namespace Transfer.Bl.MappingProfiles;

public class BaseProfile : Profile
{
    public BaseProfile()
    {
        CreateMap<DbOrganisation, CarrierSearchResultItem>()
            .ForMember(x => x.Name, opt => opt.MapFrom(o => string.IsNullOrEmpty(o.Name) ? o.FullName : o.Name))
            .ForMember(x => x.ContactFio, opt => opt.MapFrom(o => "Евгений"))
            .ForMember(x => x.ContactEmail, opt => opt.MapFrom(o => "evgen6654@mail.ru"))
            .ForMember(x => x.ContactPhone, opt => opt.MapFrom(o => "+7 916 789 58 98"));

        CreateMap<DbTripOption, TripOption>();

        CreateMap<DbTripRequest, TripRequestSearchResultItem>()
            .ForMember(x => x.TripOptions, opt => opt.MapFrom(o => o.TripOptions.Select(a => a.TripOption).ToList()))
            .ForMember(x => x.ContactFio, opt => opt.MapFrom(o => "Евгений"))
            .ForMember(x => x.ContactEmail, opt => opt.MapFrom(o => "evgen6654@mail.ru"))
            .ForMember(x => x.ContactPhone, opt => opt.MapFrom(o => "+7 916 789 58 98"));

        CreateMap<DbFile, FileDto>()
            .ForMember(x => x.Path, opt => opt.MapFrom(o => $"{o.DateCreated.Year}/{o.Id}.{o.Extention}"));

        CreateMap<DbOrganisation, OrganisationDto>();
        CreateMap<OrganisationDto, DbOrganisation>();

        CreateMap<DbOrganisation, CarrierDto>()
            .ForMember(x => x.ContactFio, opt => opt.MapFrom(o => string.IsNullOrWhiteSpace(o.DirectorFio) ? "Контактное лицо" : $"{o.DirectorFio} ({o.DirectorPosition})"));
        CreateMap<CarrierDto, DbOrganisation>();

        CreateMap<DbBankDetails, CarrierDto>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.BankInn, opt => opt.MapFrom(o => o.Inn));
        CreateMap<CarrierDto, DbBankDetails>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Inn, opt => opt.MapFrom(o => o.BankInn));


        //менять персданные водителей
        //CreateMap<DbDriver, DbPersonData>();


    }
}
