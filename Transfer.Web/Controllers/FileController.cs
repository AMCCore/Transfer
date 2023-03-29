using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Dal.Entities;
using Transfer.Common.Settings;
using System.Collections.Generic;
using MimeTypes;

namespace Transfer.Web.Controllers;

[Authorize]
public class FileController : BaseController
{
    private readonly IWebHostEnvironment _appEnvironment;

    public FileController(IWebHostEnvironment webHostEnvironment, IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<FileController> logger, IMapper mapper) : base(transferSettings, unitOfWork, logger, mapper)
    {
        _appEnvironment = webHostEnvironment;
    }

    [HttpPost]
    public async Task<Guid> UploadFile(IFormFile uploadedFile)
    {
        if (uploadedFile != null)
        {
            var fileId = Guid.NewGuid();
            var fileDate = DateTime.Now;
            var fileExtention = GetFileExtention(uploadedFile.FileName);

            var folder = Path.GetFullPath($"{_appEnvironment.WebRootPath}{TransferSettings.FileStoragePath}/{fileDate.Year}/");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string path = $"{folder}{fileId}.{fileExtention}";
            using var fileStream = new FileStream(path, FileMode.Create);

            await UnitOfWork.AddEntityAsync(new DbFile
            {
                Id = fileId,
                DateCreated = fileDate,
                Size = uploadedFile.Length,
                Extention = fileExtention,
                ContentType = uploadedFile.ContentType,
            }, CancellationToken.None);

            await uploadedFile.CopyToAsync(fileStream);

            return fileId;
        }
        throw new ArgumentNullException(nameof(uploadedFile));
    }

    public async Task<IActionResult> UploadAnyFile()
    {
        var file = Request.Form.Files.FirstOrDefault();
        if (file != null)
        {
            var fileId = await UploadFile(file);
            return Json(new { fileId, fileName = file.FileName });
        }
        throw new NotSupportedException();
    }

    private static string GetFileExtention(string fileName)
    {
        return fileName.Split('.')[^1];
    }

    [HttpGet]
    public async Task<IActionResult> GetFile([FromQuery][Required] Guid fileId, CancellationToken token = default)
    {
        //var entitys = await UnitOfWork.GetSet<DbFile>().Where(x => !x.OrganisationFiles.Any() && !x.DriverFiles.Any() && !x.BusFiles.Any() && x.DateCreated <= DateTime.Now.AddDays(-33)).ToListAsync(token);
        //foreach(var e in entitys)
        //{
        //    var path = $"{_appEnvironment.WebRootPath}{TransferSettings.FileStoragePath}/{e.DateCreated.Year}/{e.Id}.{e.Extention}";
        //    if(System.IO.File.Exists(path))
        //    {
        //        System.IO.File.Delete(path);
        //    }
        //    await UnitOfWork.DeleteAsync(e, token);
        //}

        var entity = await UnitOfWork.GetSet<DbFile>().FirstOrDefaultAsync(a => a.Id == fileId, token);
        if (entity != null)
        {
            var path = $"{_appEnvironment.WebRootPath}{TransferSettings.FileStoragePath}/{entity.DateCreated.Year}/{entity.Id}.{entity.Extention}";
            using var fileStream = System.IO.File.OpenRead(path);
            using var ms = new MemoryStream();
            fileStream.CopyTo(ms);
            var byts = ms.ToArray();
            return File(byts, entity.ContentType ?? "application/octet-stream", $"{entity.Id}.{entity.Extention}");
        }
        return NotFound();
    }

    private static readonly Dictionary<string, string> static_lib = new()
    {
        { "sd9dzh", "Договор ВОУ (агентское вознаграждение).pdf" },
        { "6thrai", "Лицензионное соглашение.pdf" },
        { "v8ls7e", "Политика конфиденциальности.pdf" },
        { "5r125k", "Партнерское соглашение на осуществление перевозок.pdf" },
        { "62s5ut", "Договор ВОУ (абонентская плата).pdf" },
        { "ibh87t", "Оферта о заключении договора фрахтования транспортных средств для перевозки пассажиров и багажа с использованием Платформы.pdf" },
        { "4ks6wz", "Соглашение об использовании Платформы.pdf" }
    };

    [HttpGet]
    [Route("static/{code}")]
    [AllowAnonymous]
    public IActionResult GetStatic(string code)
    {
        if(static_lib.ContainsKey(code))
        {
            var ext = GetFileExtention(static_lib[code]);
            var path = $"{_appEnvironment.WebRootPath}{TransferSettings.FileStoragePath}/Static/{code}.{ext}";
            if(System.IO.File.Exists(path))
            {
                using var fileStream = System.IO.File.OpenRead(path);
                using var ms = new MemoryStream();
                fileStream.CopyTo(ms);
                var byts = ms.ToArray();
                return File(byts, MimeTypeMap.GetMimeType(ext));
            }
        }
        
        return NotFound();
    }
}
