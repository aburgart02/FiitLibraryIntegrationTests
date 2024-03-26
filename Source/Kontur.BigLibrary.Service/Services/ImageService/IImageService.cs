using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;

namespace Kontur.BigLibrary.Service.Services.ImageService
{
    public interface IImageService
    {
        Task<Image> GetAsync(int id, ScaleOptions options, CancellationToken cancellation);
        Task<Image> SaveAsync(Image image, CancellationToken cancellation);
    }
}