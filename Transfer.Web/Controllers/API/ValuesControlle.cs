using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Transfer.Common;

namespace Transfer.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly TransferSettings _transferSettings;

        public ValuesController(IWebHostEnvironment webHostEnvironment, IOptions<TransferSettings> transferSettings)
        {
            _appEnvironment = webHostEnvironment;
            _transferSettings = transferSettings.Value;
        }        

        [HttpGet]
        [Route("GetData/{someData}")]
        public async Task<long> GetSomeData([Required] long someData)
        {
            var res = await Task.Run(() => someData * 2);
            return res;
        }

        [HttpPost]
        [Route("UploadFile")]
        public async Task<Guid> UploadFile(IFormFile uploadedFile)
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
}
