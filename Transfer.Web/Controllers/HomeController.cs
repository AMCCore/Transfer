﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Dal.Entities;
using Transfer.Web.Models;

namespace Transfer.Web.Controllers
{
    public class HomeController : BaseController
    {
        //private readonly ILogger<HomeController> _logger;
        //private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork, IOptions<TransferSettings> settings, ILogger<HomeController> logger)
            : base(settings, unitOfWork, logger)
        {
        }

        public IActionResult Index()
        {
            Logger?.LogInformation("Index called");

            //var accounts = UnitOfWork.GetSet<DbAccount>().Count();
            var btc = TransferSettings.TGBotToken;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
