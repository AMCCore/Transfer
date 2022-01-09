using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using Transfer.Common.Security;

namespace Transfer.Web.Models
{
    public static class Security
    {
        private static ISecurityService _securityService;

        public static void Configure(ISecurityService securityService)
        {
            _securityService = securityService;
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
    }
}
