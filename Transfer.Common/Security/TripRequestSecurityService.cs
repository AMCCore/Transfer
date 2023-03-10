using Microsoft.AspNetCore.Http;
using Transfer.Common.Enums.AccessRights;
using Transfer.Common.Extensions;

namespace Transfer.Common.Security;

public class TripRequestSecurityService : SecurityService, ITripRequestSecurityService
{
    public TripRequestSecurityService(IHttpContextAccessor contextAccessor) : base (contextAccessor)
    {
    }

    protected override bool GetAdmin()
    {
        if (base.GetAdmin())
        {
            return true;
        }

        return Rights.Any(x => x.Value.Any(y => y == TripRequestRights.TripRequestAdmin.GetEnumGuid()));
    }
}
