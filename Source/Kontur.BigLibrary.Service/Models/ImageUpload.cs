using Microsoft.AspNetCore.Http;

namespace Kontur.BigLibrary.Service.Models
{
    public class ImageUploadModel
    {
        public int? Id { get; set; }
        public IFormFile File { get; set; }
    }
}