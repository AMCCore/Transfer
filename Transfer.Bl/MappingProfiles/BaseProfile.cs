using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Bl.Dto;
using Transfer.Bl.Dto.Bus;
using Transfer.Bl.Dto.Carrier;
using Transfer.Bl.Dto.Driver;
using Transfer.Bl.Dto.TripRequest;
using Transfer.Common.Extensions;
using Transfer.Common.Enums;
using Transfer.Dal.Entities;

namespace Transfer.Bl.MappingProfiles;

public class BaseProfile : Profile
{
    public BaseProfile()
    {
        CreateMap<DbOrganisation, CarrierSearchResultItem>()
            .ForMember(x => x.Name, opt => opt.MapFrom(o => string.IsNullOrEmpty(o.Name) ? o.FullName : o.Name))
            .ForMember(x => x.Picture, opt => opt.MapFrom(o => o.Files.Where(p => !p.IsDeleted && p.FileType == OrganisationFileType.Logo).OrderBy(x => x.DateCreated).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.ContactFio, opt => opt.MapFrom(o => o.DirectorFio))
            .ForMember(x => x.ContactEmail, opt => opt.MapFrom(o => o.Email))
            .ForMember(x => x.ContactPhone, opt => opt.MapFrom(o => o.Phone));

        CreateMap<DbOrganisation, TripRequestSearchOrganisationDto>()
            .ForMember(x => x.OrganisationId, opt => opt.MapFrom(o => o.Id))
            .ForMember(x => x.OrganisationName, opt => opt.MapFrom(o => o.Name))
            .ForMember(x => x.ContactEmail, opt => opt.MapFrom(o => o.Email))
            .ForMember(x => x.ContactFio, opt => opt.MapFrom(o => o.DirectorFio))
            .ForMember(x => x.ContactPhone, opt => opt.MapFrom(o => o.Phone));

        CreateMap<DbTripOption, TripOption>();

        CreateMap<DbTripRequest, TripRequestSearchResultItem>()
            .ForMember(x => x.TripOptions, opt => opt.MapFrom(o => o.TripOptions.Select(a => a.TripOption).ToList()))
            .ForMember(x => x.Picture, opt => opt.MapFrom(o => GetCarrierLogoId(o) ))
            .ForMember(x => x.Name, opt => opt.MapFrom(o => !o.ChartererId.IsNullOrEmpty() ? o.Charterer.Name : o.СhartererName))
            .ForMember(x => x.ContactFio, opt => opt.MapFrom(o => !o.ChartererId.IsNullOrEmpty() ? o.Charterer.DirectorFio : o.ContactFio))
            .ForMember(x => x.ContactEmail, opt => opt.MapFrom(o => !o.ChartererId.IsNullOrEmpty() ? o.Charterer.Email : o.ContactEmail))
            .ForMember(x => x.ContactPhone, opt => opt.MapFrom(o => !o.ChartererId.IsNullOrEmpty() ? o.Charterer.Phone : o.ContactPhone));

        CreateMap<DbTripRequest, TripRequestDto>()
            .ForMember(x => x.ChildTrip, opt => opt.MapFrom(o => o.TripOptions.Any(z => z.TripOptionId == TripOptions.ChildTrip.GetEnumGuid())))
            .ForMember(x => x.StandTrip, opt => opt.MapFrom(o => o.TripOptions.Any(z => z.TripOptionId == TripOptions.IdleTrip.GetEnumGuid())))
            .ForMember(x => x.PaymentType, opt => opt.MapFrom(o => TripPaymentConvert(o.TripOptions)))
            .ForMember(x => x.ChartererName, opt => opt.MapFrom(o => !o.ChartererId.IsNullOrEmpty() ? o.Charterer.Name : o.СhartererName))
            .ForMember(x => x.ContactFio, opt => opt.MapFrom(o => !o.ChartererId.IsNullOrEmpty() ? o.Charterer.DirectorFio : o.ContactFio))
            .ForMember(x => x.ContactEmail, opt => opt.MapFrom(o => !o.ChartererId.IsNullOrEmpty() ? o.Charterer.Email : o.ContactEmail))
            .ForMember(x => x.ContactPhone, opt => opt.MapFrom(o => !o.ChartererId.IsNullOrEmpty() ? o.Charterer.Phone : o.ContactPhone))
            .ForMember(x => x.State, opt => opt.MapFrom(o => o.State.GetEnumGuid()));

        CreateMap<TripRequestDto, DbTripRequest>()
            .ForMember(x => x.ContactFio, opt => opt.MapFrom(o => !o.ChartererId.IsNullOrEmpty() ? null : o.ContactFio))
            .ForMember(x => x.ContactEmail, opt => opt.MapFrom(o => !o.ChartererId.IsNullOrEmpty() ? null : o.ContactEmail))
            .ForMember(x => x.ContactPhone, opt => opt.MapFrom(o => !o.ChartererId.IsNullOrEmpty() ? null : o.ContactPhone))
            .ForMember(x => x.СhartererName, opt => opt.MapFrom(o => !o.ChartererId.IsNullOrEmpty() ? null : o.ChartererName))
            .ForMember(x => x.State, opt => opt.Ignore())
            .ForMember(x => x.TripOptions, opt => opt.Ignore());

        CreateMap<DbFile, FileDto>()
            .ForMember(x => x.Path, opt => opt.MapFrom(o => $"{o.DateCreated.Year}/{o.Id}.{o.Extention}"));

        CreateMap<DbOrganisation, OrganisationDto>()
            .ForMember(x => x.State, opt => opt.MapFrom(o => o.State.GetEnumGuid()));
        CreateMap<OrganisationDto, DbOrganisation>()
            .ForMember(x => x.State, opt => opt.Ignore());

        CreateMap<DbOrganisation, CarrierDto>()
            .ForMember(x => x.LogoFileId, opt => opt.MapFrom(o => o.Files.Where(p => !p.IsDeleted && p.FileType == OrganisationFileType.Logo).OrderBy(x => x.DateCreated).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.LicenceFileId, opt => opt.MapFrom(o => o.Files.Where(p => !p.IsDeleted && p.FileType == OrganisationFileType.License).OrderBy(x => x.DateCreated).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.ContactFio, opt => opt.MapFrom(o => string.IsNullOrWhiteSpace(o.DirectorFio) ? "Контактное лицо" : $"{o.DirectorFio} ({o.DirectorPosition})"));
        CreateMap<CarrierDto, DbOrganisation>();


        CreateMap<DbBankDetails, CarrierDto>()
            .ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<CarrierDto, DbBankDetails>()
            .ForMember(x => x.Id, opt => opt.Ignore());


        CreateMap<DbDriver, OrganisationAssetDto>()
            .ForMember(x => x.Picture, opt => opt.MapFrom(o => o.DriverFiles.Where(p => !p.IsDeleted && p.FileType == DriverFileType.Avatar).OrderBy(x => x.DateCreated).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.Name, opt => opt.MapFrom(o => $"{o.LastName} {o.FirstName} {o.MiddleName}".Trim()));

        CreateMap<DbBus, OrganisationAssetDto>()
            .ForMember(x => x.Name, opt => opt.MapFrom(o => $"{o.Make} {o.Model}, {o.Yaer} гв."))
            .ForMember(x => x.Picture, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == BusFileType.PhotoMain).OrderBy(x => x.DateCreated).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.TransportClass, opt => opt.MapFrom(o => $"Название класса транспортного средства"));

        CreateMap<DbBus, BusDto>()
            .ForMember(x => x.OsagoFileId, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == BusFileType.Inshurance).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.RegFileId, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == BusFileType.Reg).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.ToFileId, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == BusFileType.TO).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.OsgopFileId, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == BusFileType.Osgop).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.TahografFileId, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == BusFileType.Tahograf).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.Photo1, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == BusFileType.PhotoMain).OrderBy(x => x.DateCreated).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.Photo2, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == BusFileType.Photo).OrderBy(x => x.DateCreated).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.Photo3, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == BusFileType.Photo).OrderBy(x => x.DateCreated).Select(p => p.FileId).Skip(1).FirstOrDefault()))
            .ForMember(x => x.Photo4, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == BusFileType.Photo).OrderBy(x => x.DateCreated).Select(p => p.FileId).Skip(2).FirstOrDefault()))
            .ForMember(x => x.Photo5, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == BusFileType.Photo).OrderBy(x => x.DateCreated).Select(p => p.FileId).Skip(3).FirstOrDefault()))
            .ForMember(x => x.Photo6, opt => opt.MapFrom(o => o.BusFiles.Where(p => !p.IsDeleted && p.FileType == BusFileType.Photo).OrderBy(x => x.DateCreated).Select(p => p.FileId).Skip(5).FirstOrDefault()))
            .ForMember(x => x.State, opt => opt.MapFrom(o => o.State.GetEnumGuid()))
            .ForMember(x => x.OrganisationName, opt => opt.MapFrom(o => o.Organisation != null ? o.Organisation.Name : null));

        CreateMap<BusDto, DbBus>()
            .ForMember(x => x.OrganisationId, opt => opt.Ignore())
            .ForMember(x => x.State, opt => opt.Ignore())
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

        CreateMap<DbDriver, DriverDto>()
            .ForMember(x => x.OrganisationName, opt => opt.MapFrom(o => o.Organisation != null ? o.Organisation.Name : null))
            .ForMember(x => x.License1, opt => opt.MapFrom(o => o.DriverFiles.Where(p => !p.IsDeleted && p.FileType == DriverFileType.License).OrderBy(x => x.DateCreated).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.License2, opt => opt.MapFrom(o => o.DriverFiles.Where(p => !p.IsDeleted && p.FileType == DriverFileType.LicenseBack).OrderBy(x => x.DateCreated).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.TahografFileId, opt => opt.MapFrom(o => o.DriverFiles.Where(p => !p.IsDeleted && p.FileType == DriverFileType.TahografCard).OrderBy(x => x.DateCreated).Select(p => p.FileId).FirstOrDefault()))
            .ForMember(x => x.State, opt => opt.MapFrom(o => o.State.GetEnumGuid()))
            .ForMember(x => x.Avatar, opt => opt.MapFrom(o => o.DriverFiles.Where(p => !p.IsDeleted && p.FileType == DriverFileType.Avatar).OrderBy(x => x.DateCreated).Select(p => p.FileId).FirstOrDefault()));

        CreateMap<DriverDto, DbDriver>()
            .ForMember(x => x.OrganisationId, opt => opt.Ignore())
            .ForMember(x => x.State, opt => opt.Ignore())
            .ForMember(x => x.DriverFiles, opt => opt.Ignore());
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

    private static int TripPaymentConvert(ICollection<DbTripRequestOption> options)
    {
        if (options.Any(x => x.TripOptionId == TripOptions.CardPayment.GetEnumGuid()))
        {
            return (int)PaymentType.Card;
        }
        else if (options.Any(x => x.TripOptionId == TripOptions.CashPayment.GetEnumGuid()))
        {
            return (int)PaymentType.Cash;
        }

        return (int)PaymentType.Cash;
    }

    private static Guid? GetCarrierLogoId(DbTripRequest tr)
    {
        return tr.Charterer?.Files?.Where(p => !p.IsDeleted && p.FileType == OrganisationFileType.Logo).OrderBy(x => x.DateCreated).Select(p => p.FileId).FirstOrDefault() ?? null;
    }
}
