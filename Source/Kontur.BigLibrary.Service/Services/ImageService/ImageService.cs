using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Services.ImageService.Repository;

namespace Kontur.BigLibrary.Service.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository imageRepository;
        private readonly IImageTransformer imageTransformer;
        private readonly int startId = 1;

        public ImageService(IImageRepository imageRepository, IImageTransformer imageTransformer)
        {
            this.imageRepository = imageRepository;
            this.imageTransformer = imageTransformer;
        }

        public async Task<Image> GetAsync(int id, ScaleOptions options, CancellationToken cancellation)
        {
            var image = await imageRepository.GetAsync(id, cancellation);

            if (image != null && options != null)
            {
                image.Data = imageTransformer.Scale(image.Data, options);
            }

            return image;
        }

        public async Task<Image> SaveAsync(Image image, CancellationToken cancellation)
        {
            image.Id ??= await GetNextBookIdAsync(cancellation);
            return await imageRepository.SaveAsync(image, cancellation);
        }

        private async Task<int> GetNextBookIdAsync(CancellationToken cancellation)
        {
            var maxId = await imageRepository.GetMaxImageIdAsync(cancellation);
            return (maxId ?? startId) + 1;
        }
    }
}