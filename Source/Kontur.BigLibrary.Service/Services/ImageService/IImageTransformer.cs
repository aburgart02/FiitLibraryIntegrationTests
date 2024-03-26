namespace Kontur.BigLibrary.Service.Services.ImageService
{
    public interface IImageTransformer
    {
        byte[] Scale(byte[] image, ScaleOptions options);
    }
}