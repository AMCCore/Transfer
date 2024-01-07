using Microsoft.AspNetCore.Http;
using System.Linq;
using Transfer.Common.Enums.AccessRights;
using Transfer.Common.Extensions;
using Transfer.Common.Security;

namespace Transfer.Web.Services;

public class TripRequestSecurityService : SecurityService, ITripRequestSecurityService
{
    public TripRequestSecurityService(IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
    }

    protected override bool GetAdmin()
    {
        if (base.GetAdmin())
        {
            return true;
        }

        return Rights.Any(x => x.Value.Any(y => y == TripRequestRights.Admin.GetEnumGuid()));
    }
}