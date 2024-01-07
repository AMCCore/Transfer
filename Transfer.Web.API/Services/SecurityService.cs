using System.Security.Claims;
using Transfer.Common.Extensions;
using Transfer.Common.Security;

namespace Transfer.Web.API.Services;

public class SecurityService : ISecurityService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public SecurityService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Guid CurrentAccountId
    {
        get
        {
            var claim = _contextAccessor.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            return claim == null ? Guid.Empty : Guid.Parse(claim.Value);
        }
    }

    public bool IsAuthenticated => !CurrentAccountId.IsNullOrEmpty();
}
