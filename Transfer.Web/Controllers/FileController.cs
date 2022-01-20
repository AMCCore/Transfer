using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Dal.Entities;

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

            string path = $"{TransferSettings.FileStoragePath}/{fileDate.Year}/{fileId}.{fileExtention}";
            using var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create);


            await UnitOfWork.AddEntityAsync(new DbFile
            {
                Id = fileId,
                DateCreated = fileDate,
                Size = uploadedFile.Length,
                Extention = fileExtention,
            }, CancellationToken.None);

            await uploadedFile.CopyToAsync(fileStream);

            return fileId;
        }
        throw new ArgumentNullException(nameof(uploadedFile));
    }

    private static string GetFileExtention(string fileName)
    {
        return fileName.Split('.')[^1];
    }
}

