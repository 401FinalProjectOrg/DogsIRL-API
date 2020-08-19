using System;
using System.IO;
using System.Threading.Tasks;
using DogsIRL_API.Models;
using DogsIRL_API.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DogsIRL_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ImageController : Controller
    {
        private IConfiguration _configuration;


        public ImageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("UploadImage")]
        public async Task<string> UploadImage(IFormFile file)
        {
            var filePath = Path.GetTempFileName();
            // stream io to save to file location
            using(var stream = System.IO.File.Create(filePath))
            {
              await file.CopyToAsync(stream);
            }
            
            string URL = await UploadToBlob(filePath);
            return URL;
        }


        private async Task<string> UploadToBlob(string filePath)
        {
            Blob blob = new Blob(_configuration);
            var name = Guid.NewGuid().ToString();

            // take the file at temp location to put into the blob storage
            await blob.UploadFile("dogsirl", name, filePath);

            // gets the blob from the storage, gives it an address
            var resultBlob = await blob.GetBlob(name, "dogsirl");
            return resultBlob.Uri.ToString();
        }
    }
}
