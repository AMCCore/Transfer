using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Transfer.Bl.Dto;
using Transfer.Common;
using Transfer.Dal.Entities;

namespace Transfer.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly TransferSettings _transferSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public ValuesController(IWebHostEnvironment webHostEnvironment, IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _appEnvironment = webHostEnvironment;
            _transferSettings = transferSettings.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetData/{someData}")]
        public async Task<long> GetSomeData([Required] long someData)
        {
            var res = await Task.Run(() => someData * 2);
            return res;
        }

        [HttpPost]
        [Route("two-files")]
        public async Task<string> Upload(IFormFile file1)
        {
            // validate the files, scan virus, save them to a file storage
            //var folder = $"{_appEnvironment.WebRootPath}{_transferSettings.FileStoragePath}/{DateTime.Now.Year}/";

            var fileId = Guid.NewGuid();
            var fileDate = DateTime.Now;
            var fileExtention = GetFileExtention(file1.FileName);

            var folder = Path.GetFullPath($"{_appEnvironment.WebRootPath}{_transferSettings.FileStoragePath}/{fileDate.Year}/");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string path = $"{folder}{fileId}.{fileExtention}";
            using var fileStream = new FileStream(path, FileMode.Create);


            await file1.CopyToAsync(fileStream);


            return folder;
        }

        [HttpPost]
        [Route(nameof(FileUpload))]
        private async Task<Guid> FileUpload(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                var fileId = Guid.NewGuid();
                var fileDate = DateTime.Now;
                var fileExtention = GetFileExtention(uploadedFile.FileName);

                var folder = $"{_appEnvironment.WebRootPath}{_transferSettings.FileStoragePath}/{fileDate.Year}/";
                if(!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string path = $"{folder}{fileId}.{fileExtention}";
                using var fileStream = new FileStream(path, FileMode.Create);

                await _unitOfWork.AddEntityAsync(new DbFile
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

        [HttpGet]
        [Route(nameof(FileGet))]
        public async Task<FileDto> FileGet([Required] [FromQuery] Guid fileId)
        {
            var entity = await _unitOfWork.GetSet<DbFile>().FirstOrDefaultAsync(a => a.Id == fileId, CancellationToken.None);
            if (entity != null)
            {
                return _mapper.Map<FileDto>(entity);
            }
            throw new FileNotFoundException();
        }
    }
}
