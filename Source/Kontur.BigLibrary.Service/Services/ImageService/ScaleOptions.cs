namespace Kontur.BigLibrary.Service.Services.ImageService
{
    public class ScaleOptions
    {
        public ScaleType Type { get; set; }
        public int Size { get; set; }
    }

    public enum ScaleType
    {
        ByWidth,
        ByByHeight
    }
}