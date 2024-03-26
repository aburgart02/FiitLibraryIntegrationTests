using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Filters;
using Kontur.BigLibrary.Service.Services.ImageService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Kontur.BigLibrary.Service.Models;

namespace Kontur.BigLibrary.Service.Controllers
{

    [Route("api/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService imageService;

        private readonly ScaleOptions defaultSmallScale = new ScaleOptions()
        {
            Size = 170,
            Type = ScaleType.ByWidth
        };

        public ImageController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<int>> Upload([FromForm] ImageUploadModel imageUpload)
        {
            if (imageUpload.File == null)
            {
                return BadRequest("No image file provided.");
            }

            using (var ms = new MemoryStream())
            {
                await imageUpload.File.CopyToAsync(ms);

                var image = new Image()
                {
                    Id = imageUpload.Id,
                    Data = ms.ToArray()
                };

                var result = await imageService.SaveAsync(image, CancellationToken.None);

                return Ok(result.Id);
            }
        }

        [HttpGet("{id}")]
        [ValidateNotEmptyResult]
        public async Task<ActionResult> Get(int id, string size = "l")
        {
            var options = size == "m" ? defaultSmallScale : null;

            var image = await imageService.GetAsync(id, options, CancellationToken.None);

            if (image != null)
            {
                return File(image.Data, "image/png");
            }

            return Ok(null);
        }
    }
}