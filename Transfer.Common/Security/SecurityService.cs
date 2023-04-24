using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common.Enums.AccessRights;
using Transfer.Common.Extensions;

namespace Transfer.Common.Security
{
    public class SecurityService : ISecurityService
    {
        private const string CurrentAccountRightsKey = "CurrentAccountRightsKey";
        private const string CurrentAccountIdKey = "CurrentAccountIdKey";
        private const string CurrentAccountOrganisationIdKey = "CurrentAccountOrganisationIdKey";
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

        public Guid? CurrentAccountOrganisationId
        {
            get
            {
                var claim = _contextAccessor.HttpContext.User?.Claims.FirstOrDefault(x =>
                    x.Type == ClaimTypes.Locality);

                return Guid.Parse(claim?.Value ?? Guid.Empty.ToString());
            }
        }

        public bool IsAuthenticated => !CurrentAccountId.IsNullOrEmpty();

        public bool IsAdmin => GetAdmin();

        public bool HasRightForSomeOrganisation(Enum right, Guid? organisation = null)
        {
            return HasRightForSomeOrganisation(right.GetEnumGuid(), organisation);
        }

        public bool HasRightForSomeOrganisation(Guid right, Guid? organisation = null)
        {
            if (IsAdmin)
            {
                return true;
            }

            if(organisation.HasValue)
            {
                return Rights.Any(s => (s.Key == organisation.Value || s.Key == Guid.Empty) && s.Value.Any(x => x == right));
            }

            return Rights.Any(s => s.Value.Any(x => x == right));
        }

        public Guid[] HasOrganisationsForRight(Enum right)
        {
            return Rights.Where(ss => ss.Key != Guid.Empty && ss.Value.Any(x => x == right.GetEnumGuid()))
                .Select(ss => ss.Key)
                .ToArray();
        }

        private IDictionary<Guid, IList<Guid>> _rights;

        public IDictionary<Guid, IList<Guid>> Rights
        {
            get
            {
                var claim = _contextAccessor.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
                if (claim == null)
                {
                    return new Dictionary<Guid, IList<Guid>>();
                }
                else
                {
                    var roles = JsonConvert.DeserializeObject<IDictionary<Guid, IList<Guid>>>(claim.Value);
                    //_contextAccessor.HttpContext.Items[CurrentAccountRightsKey] = roles;
                    return roles ?? new Dictionary<Guid, IList<Guid>>();
                }
            }
        }

        public bool HasAnyRightForSomeOrganisation(IEnumerable<Enum> rights, Guid? organisation = null)
        {
            if(IsAdmin)
            {
                return true;
            }

            return Rights.Any(s => (s.Key == organisation || organisation.IsNullOrEmpty()) && s.Value.Any(x => rights.Any(y => y.GetEnumGuid() == x)));
        }

        protected virtual bool GetAdmin()
        {
            return Rights.Any(x => x.Value.Any(y => y == AdminAccessRights.IsAdmin.GetEnumGuid()));
        }
    }
}
