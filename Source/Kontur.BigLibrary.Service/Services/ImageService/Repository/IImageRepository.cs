using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;

namespace Kontur.BigLibrary.Service.Services.ImageService.Repository
{
    public interface IImageRepository
    {
        Task<Image> GetAsync(int id, CancellationToken cancellation);
        Task<int?> GetMaxImageIdAsync(CancellationToken cancellation);
        Task<Image> SaveAsync(Image image, CancellationToken cancellation);
    }
}