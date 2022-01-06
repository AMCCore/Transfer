using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Transfer.Common;
using Transfer.Dal;

namespace Transfer.Web.Controllers
{
    [AllowAnonymous]
    public class MigrateController : Controller
    {
        private readonly IConfiguration _settings;

        public MigrateController(IConfiguration config)
        {
            _settings = config;
        }

        /// <summary>
        ///     миграция БД
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult MigrateDatabase()
        {
            using var uw = new UnitOfWork(_settings.GetConnectionString("TransferDb"));
            
            uw.Context.Database.SetCommandTimeout(1000);
            uw.Context.Database.Migrate();
            uw.SeedData();
            
            return Ok();
        }
    }
}