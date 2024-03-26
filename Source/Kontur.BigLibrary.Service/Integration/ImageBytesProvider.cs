using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Kontur.BigLibrary.Service.Integration
{
    public static class ImageBytesProvider
    {
        
        public static async Task<byte[]> ProvideAsync(int imageId, CancellationToken stoppingToken)
        {
            var hundred = (imageId - 1) / 100;
            var folder = (hundred * 100 + 1) + "-" + ((hundred + 1) * 100);
            var path = Path.Combine(Constants.BaseImagesFolder, folder, imageId + Constants.PngFormat);
            return File.Exists(path) ? await File.ReadAllBytesAsync(path, stoppingToken) : null;
        }
    }
}