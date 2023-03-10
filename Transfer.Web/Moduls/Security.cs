using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Transfer.Bl.Dto;
using Transfer.Common.Cache;
using Transfer.Common.Security;
using Transfer.Dal;
using Transfer.Dal.Entities;

namespace Transfer.Web.Moduls;

public static class Security
{
    private static ISecurityService _securityService;
    private static ICacheService _cacheService;
    private static IConfiguration _settings;

    public static void Configure(ISecurityService securityService, ICacheService cacheService, IConfiguration configuration)
    {
        _securityService = securityService;
        _cacheService = cacheService;
        _settings = configuration;
    }

    public static ISecurityService Current
    {
        get
        {
            return _securityService;
        }
    }

    public static bool IsAuthenticated => _securityService.IsAuthenticated;

    public static Guid CurrentAccountId => _securityService.CurrentAccountId;

    public static AccountDto CurrentAccount
    {
        get
        {
            var res = _cacheService?.Get<AccountDto>(GenerateCacheKey());
            if (res == null)
            {
                using var uw = new UnitOfWork(_settings.GetConnectionString("TransferDb"));
                var acc = uw.GetSet<DbAccount>().FirstOrDefault(x => x.Id == CurrentAccountId);
                res = new AccountDto
                {
                    IsMale = acc?.PersonData?.IsMale ?? true,
                    FirstName = acc?.PersonData?.FirstName,
                    LastName = acc?.PersonData?.LastName,
                    MiddleName = acc?.PersonData?.MiddleName,
                    BirthDate = acc?.PersonData?.BirthDate,
                    Email = acc?.Email
                };
                _cacheService.Set(GenerateCacheKey(), res);
            }
            return res;
        }
    }

    public static void CurrentUserClearCache() => _cacheService?.Remove(GenerateCacheKey());

    private static string GenerateCacheKey()
    {
        return $"ef_od_tfr_CurrentAccount({CurrentAccountId})";
    }

    public static int ReviewEvents => 15;

    public static bool HasRightForSomeOrganisation(Enum right, Guid? organisation = null)
    {
        return _securityService.HasRightForSomeOrganisation(right, organisation);
    }

    public static bool HasAnyRightForSomeOrganisation(IEnumerable<Enum> rights, Guid? organisation = null)
    {
        return _securityService.HasAnyRightForSomeOrganisation(rights, organisation);
    }

}
