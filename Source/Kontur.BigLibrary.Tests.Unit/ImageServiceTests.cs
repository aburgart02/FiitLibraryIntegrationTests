using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Services.ImageService;
using Kontur.BigLibrary.Service.Services.ImageService.Repository;
using NSubstitute;
using NUnit.Framework;

namespace Kontur.BigLibrary.Tests.Unit
{
    public class ImageServiceTests
    {
        private IImageRepository imageRepository;
        private IImageService imageService;
        private IImageTransformer imageTransformer;


        [SetUp]
        public void Setup()
        {
            imageRepository = Substitute.For<IImageRepository>();
            imageTransformer = Substitute.For<IImageTransformer>();
            imageService = new ImageService(imageRepository, imageTransformer);
        }

        [Test]
        public async Task Save_NewImage_SetNewId()
        {
            var image = new Image()
            {
                Data = Array.Empty<byte>()
            };

            await imageService.SaveAsync(image, CancellationToken.None);

            Received.InOrder(async () =>
            {
                await imageRepository.GetMaxImageIdAsync(CancellationToken.None);
            });
        }
    }
}