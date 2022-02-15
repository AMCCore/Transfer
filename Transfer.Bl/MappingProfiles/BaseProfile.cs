using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Bl.Dto;
using Transfer.Bl.Dto.Bus;
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

        CreateMap<DbDriver, OrganisationAssetDto>()
            //.ForMember(x => x.CompanyId, opt => opt.MapFrom(o => o.OrganisationId))
            //.ForMember(x => x.CompanyName, opt => opt.MapFrom(o => o.Organisation != null ? o.Organisation.Name : null))
            .ForMember(x => x.Name, opt => opt.MapFrom(o => $"{o.LastName} {o.FirstName} {o.MiddleName}".Trim()));
        //.ForMember(x => x.Phone, opt => opt.MapFrom(o => o.Phone))
        //.ForMember(x => x.EMail, opt => opt.MapFrom(o => o.EMail));
        CreateMap<DbBus, OrganisationAssetDto>()
            .ForMember(x => x.Name, opt => opt.MapFrom(o => $"{o.Make} {o.Model}, {o.Yaer} гв."))
            //.ForMember(x => x.CompanyId, opt => opt.MapFrom(o => o.OrganisationId))
            //.ForMember(x => x.CompanyName, opt => opt.MapFrom(o => o.Organisation != null ? o.Organisation.Name : null))
            .ForMember(x => x.TransportClass, opt => opt.MapFrom(o => $"Название класса транспортного средства"));

        CreateMap<DbBus, DbBus>()
            .ForMember(x => x.Organisation, opt => opt.Ignore())
            .ForMember(x => x.BusFiles, opt => opt.Ignore());

        CreateMap<DbBus, BusDto>()
            .ForMember(x => x.OsagoFileId, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == Common.Enums.BusFileType.Inshurance).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.RegFileId, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == Common.Enums.BusFileType.Reg).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.ToFileId, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == Common.Enums.BusFileType.TO).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.OsgopFileId, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == Common.Enums.BusFileType.Osgop).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.TahografFileId, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == Common.Enums.BusFileType.Tahograf).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.OrganisationName, opt => opt.MapFrom(o => o.Organisation != null ? o.Organisation.Name : null));

        CreateMap<BusDto, DbBus>()
            .ForMember(x => x.OrganisationId, opt => opt.Ignore())
            .ForMember(x => x.BusFiles, opt => opt.Ignore());


        CreateMap<DbDriver, DbDriver>()
            .ForMember(x => x.Organisation, opt => opt.Ignore())
            .ForMember(x => x.DriverFiles, opt => opt.Ignore())
            .ForMember(x => x.DbDriversLicenses, opt => opt.Ignore());

        CreateMap<DbBus, BusSearchItem>()
            .ForMember(x => x.Name, opt => opt.MapFrom(o => $"{o.Make} {o.Model}, {o.Yaer} гв."))
            .ForMember(x => x.CompanyId, opt => opt.MapFrom(o => o.OrganisationId))
            .ForMember(x => x.CompanyName, opt => opt.MapFrom(o => o.Organisation != null ? o.Organisation.Name : null))
            .ForMember(x => x.OptionsInstalled, opt => opt.MapFrom(o => BusOptionsConvert(o)))
            .ForMember(x => x.TransportClass, opt => opt.MapFrom(o => $"Название класса транспортного средства"));

    }

    private static string[] BusOptionsConvert(DbBus bus)
    {
        var result = new List<string>();
        //ремни безопасности
        if (bus.TV)
        {
            result.Add("телевизор");
        }
        if (bus.AirConditioner)
        {
            result.Add("кондиционер");
        }
        if (bus.Audio)
        {
            result.Add("аудиостсиема");
        }
        if (bus.WC)
        {
            result.Add("туалет");
        }
        if (bus.Microphone)
        {
            result.Add("микрофон");
        }
        if (bus.Wifi)
        {
            result.Add("wifi");
        }



        return result.ToArray();
    }
}
