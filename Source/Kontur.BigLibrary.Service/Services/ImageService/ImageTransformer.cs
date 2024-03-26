using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Kontur.BigLibrary.Service.Services.ImageService
{
    public class ImageTransformer : IImageTransformer
    {
        public byte[] Scale(byte[] rawImage, ScaleOptions options)
        {
            using var image = Image.Load(rawImage);
            
            var newSize = GetNewSize(image.Width, image.Height, options);

            image.Mutate(x => x.Resize(newSize.Item1, newSize.Item2));

            var encoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(PngFormat.Instance);

            using var ms = new MemoryStream();
            
            image.Save(ms, encoder);
            return ms.ToArray();
        }

        private static (int, int) GetNewSize(int width, int height, ScaleOptions options)
        {
            double ratio;
            var targetHeight = options.Size;
            var targetWidth = options.Size;

            switch (options.Type)
            {
                case ScaleType.ByWidth:
                    ratio = (double) (100m * targetWidth) / width;
                    targetHeight = (int)((height / 100.0) * ratio);
                    break;
                case ScaleType.ByByHeight:
                    ratio = (double) (100m * targetHeight) / height;
                    targetWidth = (int)((width / 100.0) * ratio);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ScaleType));
            }

            return (targetWidth, targetHeight);
        }
    }
}